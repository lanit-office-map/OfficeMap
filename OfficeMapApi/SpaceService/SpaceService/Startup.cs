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
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
        var connectionFactory = new ConnectionFactory
        {
          HostName = Configuration["RabbitMQ:Connection"],
          UserName = Configuration["RabbitMQ:Username"],
          Password = Configuration["RabbitMQ:Password"]
        };
        if (Configuration["RabbitMQ:Cloud_AMQP_URL"] != null)
        {
          connectionFactory.Uri =
            new Uri(Configuration["RabbitMQ:Cloud_AMQP_URL"]);
        }

        return connectionFactory;
      });
      services.AddScoped<IRabbitMQPersistentConnection, RabbitMQPersistentConnection>();
      services.AddScoped<IOfficeServiceClient, OfficeServiceClient>();
      services.AddScoped<IWorkplaceServiceClient, WorkplaceServiceClient>();
      services.AddScoped<SpaceServiceServer>();
      services.AddHostedService<ConsumeScopedServiceHostedService>();

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.Authority = Configuration["Addresses:Backend:UserService"];
          options.Audience = "SpaceService";
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
