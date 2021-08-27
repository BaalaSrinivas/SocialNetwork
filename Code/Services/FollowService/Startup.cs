using FollowService.Events.EventHandler;
using FollowService.Events.EventModel;
using FollowService.Repository;
using MessageBus.MessageBusCore;
using MessageBus.RabbitMQ;
using MessageBusCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;

namespace FollowService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddControllers();

            //Defaults to Google authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "accounts.google.com",
                    ValidAudience = "216892140019-lvs71bvj54t4s7stp195uuhl6foggrsd.apps.googleusercontent.com",
                    ValidateAudience = true,
                    ValidateIssuer = true
                };

                options.MetadataAddress = "https://accounts.google.com/.well-known/openid-configuration";
                options.TokenValidationParameters = tokenValidationParameters;
            })
           .AddJwtBearer("IdentityServer", options =>
           {
               var tokenValidationParameters = new TokenValidationParameters
               {
                   ValidIssuer = $"{Configuration.GetValue<string>("IdentityAndAccessManagementBaseUrl")}".TrimEnd('/'),
                   ValidAudience = "BSKonnectIdentityServerID",
                   ValidateAudience = true,
                   ValidateIssuer = true
               };
               
               options.MetadataAddress = $"{Configuration.GetValue<string>("IdentityAndAccessManagementBaseUrl")}.well-known/openid-configuration";
               options.TokenValidationParameters = tokenValidationParameters;
           });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme, "IdentityServer");
                defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

            services.AddScoped<IFollowEntityRepository, FollowEntityRepository>();
            services.AddScoped<IFriendEntityRepository, FriendEntityRepository>();
            services.AddScoped<IFollowMetaDataRepository, FollowMetaDataRepository>();
            services.AddScoped<IUnitofWork, UnitofWork>();

            services.AddScoped<SqlConnection>((s) => new SqlConnection(Configuration.GetConnectionString("Default")));
            services.AddScoped<IDbTransaction>(s =>
            {
                SqlConnection conn = s.GetRequiredService<SqlConnection>();
                conn.Open();
                return conn.BeginTransaction();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FollowService", Version = "v1" });
            });

            RabbitMQConnectionInfo rabbitMQConnectionInfo = new RabbitMQConnectionInfo()
            {
                HostName = Configuration.GetSection("RabbitMq")["HostName"],
                UserName = Configuration.GetSection("RabbitMq")["UserName"],
                Password = Configuration.GetSection("RabbitMq")["Password"],
                Port = Configuration.GetSection("RabbitMq").GetValue<int>("Port")
            };

            services.AddScoped<IEventHandler<UserAddedEventModel>, UserAddedEventHandler>();

            services.AddSingleton<IQueue<UserAddedEventModel>>(s =>
            {
                return new Queue<UserAddedEventModel>(new RabbitMQCore(rabbitMQConnectionInfo), "UserCreated", s);
            });
            services.AddSingleton<IQueue<NewUserFollowEventModel>>(s =>
            {
                return new Queue<NewUserFollowEventModel>(new RabbitMQCore(rabbitMQConnectionInfo), "UserFollowing", s, false);
            });
            services.AddSingleton<IQueue<FriendRequestStateChangeEventModel>>(s =>
            {
                return new Queue<FriendRequestStateChangeEventModel>(new RabbitMQCore(rabbitMQConnectionInfo), "FriendStateChange", s, false);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FollowService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            AddEventSubscription(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var connection = serviceScope.ServiceProvider.GetRequiredService<SqlConnection>();
                InitializeDatabase(connection);
            }
        }

        private void AddEventSubscription(IApplicationBuilder app)
        {
            app.ApplicationServices.GetRequiredService<IQueue<UserAddedEventModel>>().AddSubscriber<UserAddedEventHandler>();
        }

        private void InitializeDatabase(SqlConnection sqlConnection)
        {
            string dbName = Configuration.GetValue<string>("DatabaseName");
            sqlConnection.ConnectionString = sqlConnection.ConnectionString.Replace(dbName, "master");
            sqlConnection.Open();
            var cmdText = $"select count(*) from master.dbo.sysdatabases where name='{dbName}'";
            SqlCommand sqlCommand = new SqlCommand(cmdText);
            sqlCommand.Connection = sqlConnection;
            if (Convert.ToInt32(sqlCommand.ExecuteScalar()) <= 0)
            {
                //Create DB
                sqlCommand.CommandText = $"CREATE DATABASE {dbName}";
                sqlCommand.ExecuteNonQuery();

                //Create Tables
                string script = File.ReadAllText(@"DBScripts\Script.sql");
                sqlCommand.CommandText = script;               
                sqlCommand.ExecuteNonQuery();
            }
            sqlConnection.ChangeDatabase(dbName);
            sqlConnection.Close();
        }
    }
}
