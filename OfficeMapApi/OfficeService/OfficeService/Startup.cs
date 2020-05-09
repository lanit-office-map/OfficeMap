using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OfficeService.Database;
using OfficeService.Mappers;
using OfficeService.Messaging.RabbitMQ;
using OfficeService.Messaging.RabbitMQ.Interface;
using OfficeService.Repository;
using OfficeService.Repository.Interfaces;
using OfficeService.Services;
using OfficeService.Services.Interface;
using RabbitMQ.Client;

namespace OfficeService
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
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<OfficeServiceDbContext>(options => options.UseSqlServer(connectionString));
            services.AddAutoMapper(typeof(OfficeModelsProfile));
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.AddScoped<IOfficeService, OfficesService>();
            services.AddScoped<OfficeServiceServer>();
            services.AddSingleton<IConnectionFactory, ConnectionFactory>(sp =>
            {
                return new ConnectionFactory()
                {
                    HostName = Configuration["RabbitMQConnection"],
                    UserName = Configuration["RabbitMQUsername"],
                    Password = Configuration["RabbitMQPassword"]
                };
            });
            services.AddSingleton<IRabbitMQPersistentConnection, RabbitMQPersistentConnection>();
            services.AddHostedService<ConsumeScopedServiceHostedService>();
            
            services.AddControllers();
        } 

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
