using GestorPrestamos.Data.Implementations.Excel;
using GestorPrestamos.Data.Utils;
using GestorPrestamos.Domain.Implementations;
using GestorPrestamos.Domain.Interfaces;
using GestorPrestamos.Domain.Interfaces.Repository;
using GestorPrestamos.Domain.MasterData;

namespace GestorPrestamos.Extensions
{
    public static class ServicesExtension
    {
        public static void RegisterMyCustomServices(this IServiceCollection services)
        {
            RegisterRepositories(services);
            RegisterServices(services);
            //services.AddScoped<IDeudoresDictionary, DeudoresDictionary>();
            services.AddSingleton<IDeudoresDictionary, CacheableDeudoresDictionary>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IPrestamoRepository, PrestamoExcelRepository>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ILoanReceivableService, LoanReceivableService>();
            services.AddScoped<IMasterDataService, MasterDataService>();
            
        }
    }
}
