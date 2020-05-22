using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Database;
using UserService.Database.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using IdentityServer4.Configuration;
using UserService.RabbitMQ.Servers;
using RabbitMQ.Client;
using Common.RabbitMQ.Interface;
using Common.RabbitMQ;

namespace UserService
{
  public class Startup
  {
    private readonly IdentityServerConfiguration identityServerConfiguration;
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      identityServerConfiguration = new IdentityServerConfiguration(configuration);
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      //Add db settings
      var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
      var connectionString = Configuration["ConnectionString:DefaultConnection"];
      services.AddDbContext<UserServiceDbContext>(options =>
          options.UseSqlServer(connectionString));

      services.AddIdentity<DbUser, IdentityRole>(options =>
      {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
      })
        .AddEntityFrameworkStores<UserServiceDbContext>()
        .AddDefaultTokenProviders();

      //Add authentication and authorization
      services.AddIdentityServer(
          options =>
          {
            options.UserInteraction = new UserInteractionOptions()
            {
              LogoutUrl = "/Account/Logout",
              LoginUrl = "/Account/Login",
            };
          })
        .AddOperationalStore(
          options =>
          {
            options.ConfigureDbContext = b => b.UseSqlServer(
              connectionString,
              sql => sql.MigrationsAssembly(migrationsAssembly));
          })
        .AddDeveloperSigningCredential()
        .AddInMemoryIdentityResources(
          identityServerConfiguration.IdentityResources)
        .AddInMemoryApiResources(identityServerConfiguration.ApiResources)
        .AddInMemoryClients(identityServerConfiguration.Clients)
        .AddAspNetIdentity<DbUser>();


      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.Authority = Configuration["Addresses:Backend:UserService"];
          options.Audience = "UserService";
          options.RequireHttpsMetadata = false;
          options.SaveToken = true;
        });
      services.AddAuthorization();

      //Add automapping
      services.AddAutoMapper(typeof(UserModelsProfile));
            
            // RabbitMQ
            services.AddScoped<UserServiceServer>();
            services.AddSingleton<IConnectionFactory, ConnectionFactory>(sp =>
            {
                return new ConnectionFactory()
                {
                    Uri = new System.Uri(Configuration["RabbitMQ:CLOUD_AMQP_URL"]),
                    HostName = Configuration["RabbitMQ:Connection"],
                    UserName = Configuration["RabbitMQ:Username"],
                    Password = Configuration["RabbitMQ:Password"]
                };
            });
            services.AddSingleton<IRabbitMQPersistentConnection, RabbitMQPersistentConnection>();
            services.AddHostedService<ConsumeScopedUserServiceHostedService>();

            services.AddCors(confg =>
        confg.AddPolicy("AllowAngularClient",
          p => p.WithOrigins(Configuration["Addresses:Frontend:OfficeMapUI"])
            .AllowAnyMethod()
            .AllowAnyHeader()));

      //Adds services for controllers for an API
      services.AddControllersWithViews();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseStaticFiles();
      app.UseCors("AllowAngularClient");
      app.UseRouting();

      app.UseAuthentication();
      app.UseIdentityServer();

      app.UseAuthorization();

      app.UseEndpoints(
        endpoints =>
        {
          endpoints.MapControllers();
        });
    }
  }
}