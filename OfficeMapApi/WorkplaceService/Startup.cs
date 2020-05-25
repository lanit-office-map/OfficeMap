using System;
using AutoMapper;
using Common.RabbitMQ;
using Common.RabbitMQ.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using SpaceService.Clients;
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
            var connectionString = Configuration["ConnectionString:DefaultConnection"];
            services.AddDbContext<WorkplaceServiceDbContext>(options => options.UseSqlServer(connectionString));
            services.AddAutoMapper(options =>
              {
                options.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
              },
              typeof(WorkplaceModelsProfile));
            services.AddScoped<IWorkplaceRepository, WorkplaceRepository>();
            services.AddScoped<IWorkplaceService, Services.WorkplaceService>();
            services.AddScoped<IOfficeServiceClient, OfficeServiceClient>();
            services.AddScoped<ISpaceServiceClient, SpaceServiceClient>();
            services.AddScoped<IUserServiceClient, UserServiceClient>();

            //RabbitMQ
            services.AddScoped<WorkplaceServiceServer>();
            services.AddSingleton<IConnectionFactory, ConnectionFactory>(sp =>
            {
                return new ConnectionFactory()
                {
                  Uri = new Uri(Configuration["RabbitMQ:Cloud_AMQP_URL"]),
                  HostName = Configuration["RabbitMQ:Connection"],
                  UserName = Configuration["RabbitMQ:Username"],
                  Password = Configuration["RabbitMQ:Password"]
                };
            });
            services.AddScoped<IRabbitMQPersistentConnection, RabbitMQPersistentConnection>();
            services.AddHostedService<ConsumeScopedWorkplaceServiceHostedService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                options.Authority = Configuration["Addresses:Backend:UserService"];
                options.Audience = "WorkplaceService";
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
              });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
