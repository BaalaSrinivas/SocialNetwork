using MessageBus.MessageBusCore;
using MessageBus.RabbitMQ;
using MessageBusCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            services.AddTransient<IEventHandler<ContentEventModel>, ContentEventHandler>();

            services.AddSingleton<IQueue<ContentEventModel>>(
                           s =>
                           {
                               return new Queue<ContentEventModel>(new RabbitMQCore(rabbitMQConnectionInfo), "ContentQueue", s)
                               .AddSubscriber<ContentEventHandler>();
                           });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NotificationService", Version = "v1" });
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/hub");
                endpoints.MapControllers();
            });
        }
    }
}
