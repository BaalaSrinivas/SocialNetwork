using MessageBus.MessageBusCore;
using MessageBus.RabbitMQ;
using MessageBusCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotificationService.Events.EventHandler;
using NotificationService.Events.EventModel;
using NotificationService.SignalR;

namespace NotificationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            RabbitMQConnectionInfo rabbitMQConnectionInfo = new RabbitMQConnectionInfo()
            {
                HostName = Configuration.GetSection("RabbitMq")["HostName"],
                UserName = Configuration.GetSection("RabbitMq")["UserName"],
                Password = Configuration.GetSection("RabbitMq")["Password"],
                Port = Configuration.GetSection("RabbitMq").GetValue<int>("Port")
            };

            services.AddScoped<IEventHandler<NotificationEventModel>, NotificationEventHandler>();

            services.AddSingleton<IQueue<NotificationEventModel>>(s =>
            {
                return new Queue<NotificationEventModel>(new RabbitMQCore(rabbitMQConnectionInfo), "NotificationQueue", s);
            });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NotificationService", Version = "v1" });
            });

            //Defaults to Google authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NotificationService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            AddEventSubscription(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/hub");
                endpoints.MapControllers();
            });
        }

        private void AddEventSubscription(IApplicationBuilder app)
        {
            app.ApplicationServices.GetRequiredService<IQueue<NotificationEventModel>>().AddSubscriber<NotificationEventHandler>();
        }
    }
}
