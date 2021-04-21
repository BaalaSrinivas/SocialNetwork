using ContentService.Context;
using ContentService.Repository;
using MessageBus.MessageBusCore;
using MessageBus.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
            services.AddDbContext<SqlContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
            services.AddControllers();
            

            services.AddScoped<IContentRepository, ContentRepository>();

            RabbitMQConnectionInfo rabbitMQConnectionInfo = new RabbitMQConnectionInfo()
            {
                HostName = Configuration.GetSection("RabbitMq")["HostName"],
                UserName = Configuration.GetSection("RabbitMq")["UserName"],
                Password = Configuration.GetSection("RabbitMq")["Password"],
                Port = Configuration.GetSection("RabbitMq").GetValue<int>("Port")
            };

            services.AddSingleton<IMessageBus, RabbitMQBus>(s=> { return new RabbitMQBus(new RabbitMQCore(rabbitMQConnectionInfo),"ContentQueue"); });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContentService", Version = "v1" });
            });
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            { 
                endpoints.MapControllers();
            });
        }
    }
}
