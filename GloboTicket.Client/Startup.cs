using GloboTicket.Common;
using GloboTicket.Shared;
using GloboTicket.Web.Models;
using GloboTicket.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using System;

namespace GloboTicket.Web
{
    public class Startup
    {
        private readonly IHostEnvironment environment;
        private readonly IConfiguration config;

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            config = configuration;
            this.environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var builder = services.AddControllersWithViews();

            if (environment.IsDevelopment())
                builder.AddRazorRuntimeCompilation();

            services.AddTransient<LoggingDelegatingHandler>();

            services.AddHttpClient<IEventCatalogService, EventCatalogService>(c =>
                c.BaseAddress = new Uri(config["ApiConfigs:EventCatalog:Uri"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>();
            services.AddHttpClient<IShoppingBasketService, ShoppingBasketService>(c =>
                c.BaseAddress = new Uri(config["ApiConfigs:ShoppingBasket:Uri"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>();
            services.AddHttpClient<IOrderService, OrderService>(c =>
                c.BaseAddress = new Uri(config["ApiConfigs:Order:Uri"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>();
            services.AddHttpClient<IDiscountService, DiscountService>(c =>
                c.BaseAddress = new Uri(config["ApiConfigs:Discount:Uri"]))
                .AddHttpMessageHandler<LoggingDelegatingHandler>();

            services.AddSingleton<Settings>();

            services.AddHealthChecks()
                .AddUrlGroup(new Uri($"{config["ApiConfigs:EventCatalog:Uri"]}/health/live"),
                    "Event Catalog API", HealthStatus.Degraded, timeout: TimeSpan.FromSeconds(1));
        }

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
                endpoints.MapDefaultHealthChecks();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=EventCatalog}/{action=Index}/{id?}");
            });
        }
    }
}
