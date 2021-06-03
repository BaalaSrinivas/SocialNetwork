using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserManagement.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using UserManagement.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using UserManagement.Services;
using System;

namespace UserManagement
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "User API", Version = "v1" });
            });
            services.AddDbContext<UserContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            services.AddHttpClient<IBlobService, BlobService>(u => u.BaseAddress = new Uri(Configuration.GetValue<string>("BlobServiceUrl")));
            services.AddScoped<ISMUserRepository, SMUserRepository>();

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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "User API v1"));
            }

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
