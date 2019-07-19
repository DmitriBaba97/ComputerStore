using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ComputerStore.Models;
using ComputerStore.Models.DbContexts;
using ComputerStore.Models.Repositories;
using ComputerStore.Infrastructure.Validators;
using ComputerStore.Infrastructure;
using ComputerStore.Infrastructure.Extensions;
using ComputerStore.Infrastructure.Binders;


namespace ComputerStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration config) => Configuration = config;
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["ComputerStore:ConnectionString"])
            );
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(Configuration["Users:ConnectionString"])
            );
            services.AddProductsService();
            services.AddProductPropertiesService();
            services.AddImagesService();
            services.AddCategoriesService();
            services.AddSubcategoriesService();
            services.AddFilterOptionsService();
            services.AddFilterOptionValuesService();
            services.AddImageWriterService();
            services.AddCustomPasswordValidator();
            services.AddCustomUserValidator();
            services.AddOrdersService();
            services.AddCountriesList();
            services.AddScoped<Cart>(sc => Cart.GetCart(sc));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
            services.AddMvc(options => {
                //Set custom model binder provider for invariant input of decimal value
                options.ModelBinderProviders.Insert(0, new InvariantDecimalModelBinderProvider());
            })
             .AddJsonOptions(
                //Allow access of navigation properties in json strings
            options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddMemoryCache();
            services.AddSession();
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "XSRF-TOKEN";
                options.SuppressXFrameOptionsHeader = false;
            });
        }

       
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        { 
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStackifyProfiler();
            }

            //Add logging in file
            loggerFactory.AddFile(Configuration.GetSection("Logging"));
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
