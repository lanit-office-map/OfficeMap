using System;
using System.Security.Policy;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OfficeService.Database;
using OfficeService.Mappers;
using Common.RabbitMQ.Interface;
using Common.RabbitMQ;
using OfficeService.Servers;
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
      var connectionString = Configuration["ConnectionString:DefaultConnection"];
      services.AddDbContext<OfficeServiceDbContext>(options => options.UseSqlServer(connectionString));
      services.AddAutoMapper(options =>
        {
          options.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
        },
        typeof(OfficeModelsProfile));
      services.AddScoped<IOfficeRepository, OfficeRepository>();
      services.AddScoped<IOfficeService, OfficesService>();
      services.AddScoped<OfficeServiceServer>();
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
      services.AddHostedService<ConsumeScopedServiceHostedService>();

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.Authority = Configuration["Addresses:Backend:UserService"];
          options.Audience = "OfficeService";
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

      app.UseAuthentication();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
