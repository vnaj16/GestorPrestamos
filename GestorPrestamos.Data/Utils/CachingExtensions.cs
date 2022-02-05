using GestorPrestamos.Data.Extensions;
using GestorPrestamos.Data.Implementations.Excel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Data.Utils
{
    public static class CachingExtensions
    {
        public static void ConfigureCaching(this IServiceCollection services, Action<CachingConfiguration> configure)
        {
            CachingConfiguration cachingOptions= new CachingConfiguration();
            configure(cachingOptions);
        }
    }
}
