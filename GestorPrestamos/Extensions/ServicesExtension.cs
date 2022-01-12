using GestorPrestamos.Data.Implementations.Excel;
using GestorPrestamos.Data.Interfaces;
using GestorPrestamos.Services;
using GestorPrestamos.Services.Interfaces;

namespace GestorPrestamos.Extensions
{
    public static class ServicesExtension
    {
        public static void RegisterMyCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IMyService, MyService>();
            services.AddScoped<IRepository, ExcelRepository>();
        }
    }
}
