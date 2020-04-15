
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OfficeService.Database;
using OfficeService.Mappers;
using OfficeService.Repository;
using OfficeService.Repository.Interfaces;
using OfficeService.Services.Interface;

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
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<OfficeServiceDbContext>(options => options.UseSqlServer(connectionString));
            services.AddAutoMapper(typeof(OfficeModelsProfile));
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.AddScoped<IOfficeService, Services.OfficesService>();

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
