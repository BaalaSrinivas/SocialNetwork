using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NewsfeedService.Repository;
using NewsfeedService.Services;
using StackExchange.Redis;
using System;

namespace NewsfeedService
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
            services.AddHttpClient<IContentService, ContentService>(u=> u.BaseAddress = new Uri(Configuration.GetValue<string>("ContentServiceUrl")));
            services.AddHttpClient<IFollowService, FollowService>(u => u.BaseAddress = new Uri(Configuration.GetValue<string>("FollowServiceUrl")));
            
            services.AddScoped<ICacheRepository, RedisCacheRepository>();

            services.AddSingleton<ConnectionMultiplexer>(c => {
                return ConnectionMultiplexer.Connect(Configuration.GetValue<string>("RedisConnectionString"));
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewsfeedService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NewsfeedService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
