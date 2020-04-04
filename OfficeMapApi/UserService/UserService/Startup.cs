using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Database;
using UserService.Database.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<UserServiceDBContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<DbUser, IdentityRole>()
              .AddEntityFrameworkStores<UserServiceDBContext>()
              .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddApiAuthorization<DbUser, UserServiceDBContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddAuthorization(
              options =>
              {
                options.AddPolicy("default", policy => policy.RequireAuthenticatedUser());
              });

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