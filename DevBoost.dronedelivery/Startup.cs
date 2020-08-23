using DevBoost.dronedelivery.Data.Repositories;
using DevBoost.DroneDelivery.Domain.Services;
using DevBoost.DroneDelivery.Repository.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevBoost.dronedelivery
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddDbContext<PedidoContext>();
            //services.AddTransient<IUnitOfWork, UnitOfWork>();
            //services.AddTransient(typeof(IRepository<>), typeof(EFRepository<>));
            services.AddTransient<DCDroneDelivery>();
            services.AddScoped<IServicePedido, ServicePedido>();
            services.AddScoped<IDroneItinerarioRepository, DroneItinerarioRepository>();
            services.AddScoped<IDroneRepository, DroneRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();

            services.AddDbContext<DCDroneDelivery>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    m => m.MigrationsAssembly("DroneDelivery"));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
