using System.Reflection;
using IdentityServer4.Services;
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
using Microsoft.AspNetCore.Http;

namespace UserService
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      //Add db settings
      var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
      var connectionString = Configuration.GetConnectionString("DefaultConnection");
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
              LogoutUrl = "/UserService/Account/Logout",
              LoginUrl = "/UserService/Account/Login",
              //LoginReturnUrlParameter = "ReturnUrl",
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
          IdentityServerConfiguration.IdentityResources)
        .AddInMemoryApiResources(IdentityServerConfiguration.ApiResources)
        .AddInMemoryClients(IdentityServerConfiguration.Clients)
        .AddAspNetIdentity<DbUser>();

      //services.AddTransient<IProfileService, IdentityClaimsProfileService>();

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.Authority = "http://localhost:5000";
          options.Audience = "OfficeMapAPIs";
          options.RequireHttpsMetadata = false;
          options.SaveToken = true;
        });
      services.AddAuthorization();

      //Add automapping
      services.AddAutoMapper(typeof(UserModelsProfile));

      services.AddCors(confg =>
        confg.AddPolicy("AllowAngularClient",
          p => p.WithOrigins("http://localhost:4200")
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
      app.UseCors("AllowAngularClient");
      app.UseRouting();

      app.UseAuthentication();
      app.UseIdentityServer();

      app.UseAuthorization();

      //app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

      app.UseEndpoints(
        endpoints =>
        {
          endpoints.MapControllers();
        });
    }
  }
}