using AutoMapper;
using Common.RabbitMQ;
using Common.RabbitMQ.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using WorkplaceService.Clients;
using WorkplaceService.Clients.Interfaces;
using WorkplaceService.Database;
using WorkplaceService.Mappers;
using WorkplaceService.Repository;
using WorkplaceService.Repository.Interfaces;
using WorkplaceService.Servers;
using WorkplaceService.Services;
using WorkplaceService.Services.Interfaces;

namespace WorkplaceService
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
            services.AddDbContext<WorkplaceServiceDbContext>(options => options.UseSqlServer(connectionString));
            services.AddAutoMapper(typeof(WorkplaceModelsProfile));
            services.AddScoped<IWorkplaceRepository, WorkplaceRepository>();
            services.AddScoped<IWorkplaceService, Services.WorkplaceService>();
            services.AddScoped<ISpaceServiceClient, SpaceServiceClient>();
            services.AddScoped<IUserServiceClient, UserServiceClient>();

            //RabbitMQ
            services.AddScoped<WorkplaceServiceServer>();
            services.AddSingleton<IConnectionFactory, ConnectionFactory>(sp =>
            {
                return new ConnectionFactory()
                {
                    Uri = new System.Uri(Configuration["CLOUDAMQP_URL"]),
                    HostName = Configuration["RabbitMQConnection"],
                    UserName = Configuration["RabbitMQUsername"],
                    Password = Configuration["RabbitMQPassword"]
                };
            });
            services.AddSingleton<IRabbitMQPersistentConnection, RabbitMQPersistentConnection>();
            services.AddHostedService<ConsumeScopedWorkplaceServiceHostedService>();

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
