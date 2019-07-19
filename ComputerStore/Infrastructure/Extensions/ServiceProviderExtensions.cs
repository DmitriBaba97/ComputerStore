using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputerStore.Models;
using ComputerStore.Models.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using ComputerStore.Infrastructure.Validators;

namespace ComputerStore.Infrastructure.Extensions
{
    public static class ServiceProviderExtensions
    {

        public static void AddProductsService(this IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
        }

        public static void AddProductPropertiesService(this IServiceCollection services)
        {
            services.AddTransient<IProductPropertyRepository, ProductPropertyRepository>();
        }

        public static void AddImagesService(this IServiceCollection services)
        {
            services.AddTransient<IImageRepository, ImageRepository>();
        }

        public static void AddCategoriesService(this IServiceCollection services)
        {
            services.AddTransient<ICategoryRepository, CategoryRepository>();
        }

        public static void AddSubcategoriesService(this IServiceCollection services)
        {
            services.AddTransient<ISubcategoryRepository, SubcategoryRepository>();
        }

        public static void AddFilterOptionsService(this IServiceCollection services)
        {
            services.AddTransient<IFilterOptionRepository, FilterOptionRepository>();
        }

        public static void AddFilterOptionValuesService(this IServiceCollection services)
        {
            services.AddTransient<IFilterOptionValuesRepository, FilterOptionValuesRepository>();
        }

        public static void AddImageWriterService(this IServiceCollection services)
        {
            services.AddTransient<IImageWriter, ImageWriter>();
        }

        public static void AddCustomPasswordValidator(this IServiceCollection services)
        {
            services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordValidator>();
        }

        public static void AddCustomUserValidator(this IServiceCollection services)
        {
            services.AddTransient<IUserValidator<AppUser>, CustomUserValidator>();
        }

        public static void AddOrdersService(this IServiceCollection services)
        {
            services.AddTransient<IOrderRepository, OrderRepository>();
        }

        public static void AddCountriesList(this IServiceCollection services)
        {
            services.AddSingleton<Countries>();
        }
    }
}
