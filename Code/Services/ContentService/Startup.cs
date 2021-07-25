using ContentService.Context;
using ContentService.Events.EventHandler;
using ContentService.Events.EventModel;
using ContentService.Repository;
using ContentService.Services;
using MessageBus.MessageBusCore;
using MessageBus.RabbitMQ;
using MessageBusCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;

namespace ContentService
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
            services.AddHealthChecks().AddDbContextCheck<SqlContext>();

            services.AddDbContext<SqlContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
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
                   ValidIssuer = "https://localhost:5004",
                   ValidAudience = "BSKonnectIdentityServerID",
                   ValidateAudience = true,
                   ValidateIssuer = true
               };

               options.MetadataAddress = "https://localhost:5004/.well-known/openid-configuration";
               options.TokenValidationParameters = tokenValidationParameters;
           });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme, "IdentityServer");
                defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepostitory>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IPostImageRepository, PostImageRepository>();
            services.AddTransient<IEventHandler<NewContentEventModel>, ContentEventHandler>();
            services.AddHttpClient<IBlobService, BlobService>(u => u.BaseAddress = new Uri(Configuration.GetValue<string>("BlobServiceUrl")));

            RabbitMQConnectionInfo rabbitMQConnectionInfo = new RabbitMQConnectionInfo()
            {
                HostName = Configuration.GetSection("RabbitMq")["HostName"],
                UserName = Configuration.GetSection("RabbitMq")["UserName"],
                Password = Configuration.GetSection("RabbitMq")["Password"],
                Port = Configuration.GetSection("RabbitMq").GetValue<int>("Port")
            };

            services.AddSingleton<IQueue<NewContentEventModel>>(s =>
            {
                return new Queue<NewContentEventModel>(new RabbitMQCore(rabbitMQConnectionInfo), "UserPost", s);
            });

            services.AddSingleton<IQueue<UserLikedEventModel>>(s =>
            {
                return new Queue<UserLikedEventModel>(new RabbitMQCore(rabbitMQConnectionInfo), "UserLiked", s);
            });

            services.AddSingleton<IQueue<UserCommentedEventModel>>(s =>
            {
                return new Queue<UserCommentedEventModel>(new RabbitMQCore(rabbitMQConnectionInfo), "UserCommented", s);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContentService", Version = "v1" });
            });

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContentService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
