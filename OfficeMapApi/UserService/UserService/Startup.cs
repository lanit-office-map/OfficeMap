using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Database;
using UserService.Database.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace UserService
{
    public class Startup
    {
        #region private properties
        private X509Certificate2 Certificate { get; }
        #endregion
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Certificate = new X509Certificate2(
              Path.Combine(
                Directory.GetCurrentDirectory(),
                "Certificate.pfx"),
              "secret");
    }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<UserServiceDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<DbUser, IdentityRole>()
              .AddEntityFrameworkStores<UserServiceDbContext>()
              .AddDefaultTokenProviders();

            services.AddIdentityServer()
              .AddOperationalStore(options =>
              {
                options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
              })
              .AddDeveloperSigningCredential()
              .AddInMemoryApiResources(IdentityServerConfiguration.ApiResources)
              .AddInMemoryClients(IdentityServerConfiguration.Clients)
              .AddAspNetIdentity<DbUser>();

            services.AddAuthentication(options =>
              {
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              })
              .AddIdentityServerAuthentication(options =>
              {
                options.Authority = "http://localhost:5000";
                options.ApiName = "OfficeMapAPIs";
                options.RequireHttpsMetadata = false;
              });

            services.AddAuthorization();

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