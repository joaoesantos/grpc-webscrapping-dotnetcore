using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp;
using GrpcWebScrappingService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GrpcWebScrappingService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var appBootstrapper = new AppBootstrapper();
            appBootstrapper.Bootstrap();

            if (appBootstrapper.WasInitialised)
            {
                foreach (var scrapper in appBootstrapper.Scrappers)
                {
                    Thread thread = new Thread(scrapper.Run);
                    thread.Start();
                }
                
                new Thread(
                    () =>
                    {
                        while (true)
                        {
                            appBootstrapper.DataProcessor.Process();
                        }
                    });
                
                app.UseRouting();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGrpcService<GreeterService>();
                    endpoints.MapGrpcService<ScrappingService>();

                    
                    //TODO add endpoint to share proto contract
                    endpoints.MapGet("/",
                        async context =>
                        {
                            await context.Response.WriteAsync(
                                "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                        });
                });
            }


        }
    }
}