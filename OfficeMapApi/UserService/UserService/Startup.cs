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
            services.AddIdentityServer()
              .AddOperationalStore(
                options =>
                {
                  options.ConfigureDbContext = b => b.UseSqlServer(
                    connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
                })
              .AddDeveloperSigningCredential()
              .AddInMemoryApiResources(IdentityServerConfiguration.ApiResources)
              .AddInMemoryClients(IdentityServerConfiguration.Clients)
              .AddAspNetIdentity<DbUser>();

            services.AddTransient<IProfileService, IdentityClaimsProfileService>();

            services.AddAuthentication(options =>
              {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              })
              .AddIdentityServerAuthentication(options =>
              {
                options.Authority = "http://localhost:5000";
                options.ApiName = "OfficeMapAPIs";
                options.RequireHttpsMetadata = false;
              });

            services.AddAuthorization();

            //Add automapping
            services.AddAutoMapper(typeof(UserModelsProfile));

            //Adds services for controllers for an API
            services.AddControllers();
    }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
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