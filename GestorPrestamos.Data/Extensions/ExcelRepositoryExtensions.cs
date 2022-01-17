using GestorPrestamos.Data.Implementations.Excel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Data.Extensions
{
    public static class ExcelRepositoryExtensions
    {
        public static void ConfigureExcelRepository(this IServiceCollection services, Action<ExcelRepositoryOptions> configure)
        {
            ExcelRepositoryOptions excelRepositoryOptions = new ExcelRepositoryOptions();
            configure(excelRepositoryOptions);
            ExcelRepositoryConfiguration.FilePath = excelRepositoryOptions.FilePath;
        }
    }
}
