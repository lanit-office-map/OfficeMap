using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using RabbitMQ.Client;
using SpaceService.Clients;
using SpaceService.Database.Entities;
using SpaceService.Mappers;
using Common.RabbitMQ.Interface;
using Common.RabbitMQ;
using SpaceService.Clients.Interfaces;
using SpaceService.Repository;
using SpaceService.Repository.Interfaces;
using SpaceService.Services;
using SpaceService.Services.Interfaces;
using SpaceService.Controllers;
using SpaceService.Servers;

namespace SpaceService
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
      services.AddDbContext<SpaceServiceDbContext>(options => options.UseSqlServer(connectionString));

      services.AddAutoMapper(options =>
        {
          options.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
        },
        typeof(SpaceModelsProfile));

      services.AddScoped<ISpaceTypeRepository, SpaceTypeRepository>();
      services.AddScoped<ISpaceRepository, SpaceRepository>();
      services.AddScoped<ISpacesService, SpacesService>();
      services.AddScoped<ISpaceTypeService, SpaceTypeService>();
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
      services.AddSingleton<IRabbitMQPersistentConnection, RabbitMQPersistentConnection>();
      services.AddScoped<IOfficeServiceClient, OfficeServiceClient>();
      services.AddScoped<IWorkplaceServiceClient, WorkplaceServiceClient>();
      services.AddScoped<SpaceServiceServer>();
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
