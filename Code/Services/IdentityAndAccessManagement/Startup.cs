using IdentityAndAccessManagement.Data;
using IdentityAndAccessManagement.Models;
using IdentityAndAccessManagement.Services;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Linq;
using System.Reflection;

namespace IdentityAndAccessManagement
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
            services.AddHealthChecks().AddDbContextCheck<IAMContext>();
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

            //.Net Core Identity Start
            services.AddDbContext<IAMContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
            services.AddIdentity<SocialUser, IdentityRole>(config =>
            {
                //config.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<IAMContext>();
            //.AddDefaultTokenProviders();
            //.Net Core Identity End

            //Identity Server 4 Start
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<SocialUser>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(Configuration.GetConnectionString("Default"),
                            sqlServerOptionsAction: sqlOptions =>
                            {
                                sqlOptions.MigrationsAssembly(migrationsAssembly);
                            });
                    };
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(Configuration.GetConnectionString("Default"),
                            sqlServerOptionsAction: sqlOptions =>
                            {
                                sqlOptions.MigrationsAssembly(migrationsAssembly);
                            });
                    };
                }).Services.AddScoped<IProfileService, SocialProfileService>();

            //Identity Server 4 End

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IdentityAndAccessManagement", Version = "v1" });
            });

            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IEmailService, EMailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityAndAccessManagement v1"));
            }

            app.UseCors("Cors");           

            app.UseIdentityServer();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                app.UseIdentityServer();

                var iamContext = serviceScope.ServiceProvider.GetRequiredService<IAMContext>();
                var configDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                var persistedGrantDbContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();

                ApplyMigrations(iamContext);
                ApplyMigrations(configDbContext);
                ApplyMigrations(persistedGrantDbContext);

                if (!configDbContext.Clients.Any())
                {
                    foreach (var client in new IdServerConfig(Configuration.GetValue<string>("UiUrl")).GetClients())
                    {
                        configDbContext.Clients.Add(client.ToEntity());
                    }
                    configDbContext.SaveChanges();
                }

                if (!configDbContext.IdentityResources.Any())
                {
                    foreach (var resource in IdServerConfig.IdentityResources)
                    {
                        configDbContext.IdentityResources.Add(resource.ToEntity());
                    }
                    configDbContext.SaveChanges();
                }
            }
        }

        private void ApplyMigrations(DbContext context)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
