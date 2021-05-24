using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrafficRecording.Hubs;

namespace TrafficRecording
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
            services.AddRazorPages();
            services.AddSignalR();

            services.AddCors();
            services.AddControllers();
            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddMvc();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.Use(async (context, next) =>
            {
                string bodyString;
                using (var sw = new StreamReader(context.Request.Body))
                {
                    bodyString = await sw.ReadToEndAsync();
                }

                var hubContext = context.RequestServices.GetService<IHubContext<TrafficHub>>();
                var displayUrl = context.Request.GetDisplayUrl();

                await hubContext.Clients.All.SendAsync("ReceiveMessage",
                    new
                    {
                        Url = context.Request.GetDisplayUrl(),
                        Method = context.Request.Method,
                        Body = bodyString,
                        Status = context.Response.StatusCode
                    });
                await next.Invoke();

                if (displayUrl.Contains("/BasicAuth/SendRequest") || displayUrl.Contains("/Tab/SendRequest"))
                {
                }
                else
                {
                    
                }

                
            });

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthorization();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<TrafficHub>("/trafficHub");

                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "NoAuth",
                    pattern: "NoAuth/{action}/{id}",
                    new { controller = "NoAuth", action = "Index"}
                );

                endpoints.MapControllerRoute(
                    name: "tab",
                    pattern: "tab/{action}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "basicauth",
                    pattern: "basicauth/{action}/{id?}"
                );
            });

        }
    }
}
