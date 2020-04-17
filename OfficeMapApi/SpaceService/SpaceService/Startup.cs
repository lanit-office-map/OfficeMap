using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpaceService.Database.Entities;
using SpaceService.Mappers;
using SpaceService.Repository;
using SpaceService.Repository.Interfaces;
using SpaceService.Services.Interface;

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
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<SpaceServiceDbContext>(options => options.UseSqlServer(connectionString));
            services.AddAutoMapper(typeof(SpaceModelsProfile));
            services.AddScoped<ISpaceTypeRepository, SpaceTypeRepository>();
            services.AddScoped<ISpaceTypeService, Services.SpaceTypeService>();
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
