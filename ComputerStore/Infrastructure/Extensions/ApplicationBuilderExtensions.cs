using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace ComputerStore.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseStackifyProfiler(this IApplicationBuilder app)
        {
            app.UseMiddleware<StackifyMiddleware.RequestTracerMiddleware>();
        }
    }
}
