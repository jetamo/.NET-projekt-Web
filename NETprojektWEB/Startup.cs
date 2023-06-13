using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETprojektWEB.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace NETprojektWEB
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
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddControllersWithViews();

            services.AddScoped<BookListItemsService>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
        }

        class CustomCultureProvider : IRequestCultureProvider
        {
            public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
            {
                try
                {
                    return Task.FromResult(new ProviderCultureResult(httpContext.Request.Query["lang"][0]));
                }
                catch
                {
                    return Task.FromResult(new ProviderCultureResult("cs-CZ"));
                }
            }
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
             
            app.UseRequestLocalization((RequestLocalizationOptions options) => {
                // výchozí kultůra
                options.DefaultRequestCulture = new RequestCulture("cs-CZ");
                // Kultůry, které se mohou použít pro formátování čísel, dat, atd...
                options.SupportedCultures = new List<CultureInfo>() { new CultureInfo("cs-CZ"), new CultureInfo("en-US") };
                // Kultůry, které se pohou použít pro resx soubory
                options.SupportedUICultures = new List<CultureInfo>() { new CultureInfo("cs-CZ"), new CultureInfo("en-US") };
                // Vlastní poskytovatel kultůry
                options.RequestCultureProviders.Insert(0, new CustomCultureProvider());
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
