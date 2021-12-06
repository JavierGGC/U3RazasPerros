using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using U3RazasPerros.Models;

namespace U3RazasPerros
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<perrosContext>(options =>
            {
                var connectionString = "server=localhost;user=root;password=root;database=perros";
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
            services.AddMvc();
        }

        public IWebHostEnvironment Environment { get; set; }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            Environment = env;
            app.UseRouting();
            app.UseFileServer();

            app.UseEndpoints(endpoints =>
            {
                //ruteo para admin
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Admin}/{action=Index}/{id?}");
                //ruteo para usuario
                endpoints.MapControllerRoute("default","{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
