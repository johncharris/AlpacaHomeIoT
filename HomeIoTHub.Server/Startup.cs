using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using HomeIoTHub.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet.AspNetCore;
using MQTTnet.Server;

namespace HomeIoTHub.Server
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //this adds a hosted mqtt server to the services
            services.AddHostedMqttServer(builder => builder.WithDefaultEndpointPort(1883)
            .WithApplicationMessageInterceptor(context =>
            {
                if (context.ApplicationMessage.Topic.StartsWith("dogwater"))
                {
                    BackgroundJob.Enqueue<IDogWaterService>(dogWaterService => dogWaterService.HandleDogWaterEvent(context.ApplicationMessage));
                }
            }));

            services.AddHangfire(config => config.UseMemoryStorage(new MemoryStorageOptions
            {
                CountersAggregateInterval = TimeSpan.FromMilliseconds(500)
            }
            ));

            //this adds tcp server support based on System.Net.Socket
            services.AddMqttTcpServerAdapter();

            //this adds websocket support
            services.AddMqttWebSocketServerAdapter();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IDogWaterService, DogWaterService>();
            services.AddSingleton<IFcmService, FcmService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseCookiePolicy();

            app.UseMqttEndpoint();

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            app.UseMvc();
        }
    }
}
