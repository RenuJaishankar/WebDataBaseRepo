using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebDatabase
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
            services.AddControllersWithViews();
            // AddSingleton   // global type services
            // AddScoped      // request type services
            // AddTransient   // per middleware services (every class will get its own instance)
            services.AddSingleton<Models.ICRUD>(new Models.ChinookCrud());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //  Segment1: {controller=Home}                              controller: default is Home
                //  Segment2:                   {action=Index}               action:     default is Index
                //  Segment3:                                  {id?}         id:         no default, optional(?)
            });

            // URL : /
            //     Segment1: Not Supplied  -> default of Home is applied
            //     Segment2: Not Supplied  -> default of Index is applied
            //     Segment3: Not supplied  -> id is optional: not supplied - not included
            //        final route values:   controller=Home
            //                              action=Index
            // URL : /chinook
            //     Segment1: chinook       -> default of Home is not applied since there was a match
            //     Segment2: Not Supplied  -> default of Index is applied
            //     Segment3: Not supplied  -> id is optional: not supplied - not included
            //        final route values:   controller=chinook
            //                              action=Index
            // URL : /chinook/details
            //     Segment1: chinook       -> default of Home is not applied since there was a match
            //     Segment2: details       -> default of Index is not applied since there was a match
            //     Segment3: Not supplied  -> id is optional: not supplied - not included
            //        final route values:   controller=chinook
            //                              action=details
            // URL : /Track/details/3
            //     Segment1: Track         -> default of Home is not applied since there was a match
            //     Segment2: details       -> default of Index is not applied since there was a match
            //     Segment3: 3             -> id is optional: was supplied - is included
            //        final route values:   controller=chinook
            //                              action=details
            //                              id=3
        }
    }
}
