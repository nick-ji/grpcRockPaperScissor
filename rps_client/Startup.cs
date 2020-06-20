using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using rps_client.Data;
using System.Net.Http;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

namespace rps_client
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
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            services.AddGrpcClient<Rps.Rps.RpsClient>(service =>
                service.Address = new Uri("https://localhost:5000")
            );

            // services.AddSingleton(services =>
            // {
            //     var handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
            //     // var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
            //     var channel = GrpcChannel.ForAddress("https://localhost:5000", new GrpcChannelOptions
            //     {
            //         HttpHandler = new GrpcWebHandler(new HttpClientHandler()),
            //         HttpClient = new HttpClient(handler)
            //     });
            //     // Now we can instantiate gRPC clients for this channel
            //     return new Rps.Rps.RpsClient(channel);
            // });
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseGrpcWeb();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
