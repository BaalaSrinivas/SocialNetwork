using ApiGateway.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Net.Http;

namespace ApiGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("Cors", builder =>
                {
                    builder
                        .WithOrigins(
                        "http://localhost")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod();
                });
            });

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

            services.AddHttpClient<IUserService, UserService>(u => u.BaseAddress = new Uri($"{Configuration.GetValue<string>("UserManagementBaseUrl")}{Configuration.GetValue<string>("UserControllerPath")}"));
            services.AddHttpClient<IContentService, ContentService>(u => u.BaseAddress = new Uri($"{Configuration.GetValue<string>("ContentServiceBaseUrl")}{Configuration.GetValue<string>("ContentControllerPath")}"));
            services.AddHttpClient<IFollowService, FollowService>(u => u.BaseAddress = new Uri($"{Configuration.GetValue<string>("FollowServiceBaseUrl")}{Configuration.GetValue<string>("FollowControllerPath")}"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Gateway", Version = "v1" });
            });

            services.AddOcelot();
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIGateway v1"));
            }

            app.UseCors("Cors");
            
            app.UseWebSockets();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseOcelot().Wait();
        }
    }
}
