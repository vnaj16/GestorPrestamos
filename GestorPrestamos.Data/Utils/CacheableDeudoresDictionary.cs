using GestorPrestamos.Data.Implementations.Excel;
using GestorPrestamos.Domain.Entities;
using GestorPrestamos.Domain.MasterData;
using GestorPrestamos.Domain.Utils;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Data.Utils
{
    public class CacheableDeudoresDictionary : IDeudoresDictionary, ICacheable
    {
        private Dictionary<int, Deudor> DeudoresById { get; set; }
        private Dictionary<string, Deudor> DeudoresByAlias { get; set; }
        public int LifetimeInSeconds { get; set; }

        private DateTime ExpiresAt;
        private readonly ILogger _logger;
        public CacheableDeudoresDictionary(ILogger<CacheableDeudoresDictionary> logger)
        {
            _logger = logger;
            RefreshData();
            LifetimeInSeconds = CachingConfiguration.LifetimeInSecondsForDeudoresDictionary;
            ExpiresAt = DateTime.Now.AddSeconds(LifetimeInSeconds);
            _logger.LogInformation("Expires At: " + ExpiresAt);
        }

        public Dictionary<string, Deudor> GetDeudoresByAlias()
        {
            if (ExpiresAt < DateTime.Now)
            {
                _logger.LogInformation($"Ya expiró. ExpiresAt {ExpiresAt} y Now {DateTime.Now}");
                RefreshData();
                ExpiresAt = DateTime.Now.AddSeconds(LifetimeInSeconds);
            }
            else
            {
                _logger.LogInformation($"Aún no expira. ExpiresAt {ExpiresAt} y Now {DateTime.Now}");

            }
            return DeudoresByAlias;
        }

        public Dictionary<int, Deudor> GetDeudoresById()
        {
            if (ExpiresAt < DateTime.Now)
            {
                _logger.LogInformation($"Ya expiró. ExpiresAt {ExpiresAt} y Now {DateTime.Now}");
                RefreshData();
                ExpiresAt = DateTime.Now.AddSeconds(LifetimeInSeconds);
            }
            else
            {
                _logger.LogInformation($"Aún no expira. ExpiresAt {ExpiresAt} y Now {DateTime.Now}");
            }
            return DeudoresById;
        }

        public void RefreshData()
        {
            DeudoresById = new Dictionary<int, Deudor>();
            DeudoresByAlias = new Dictionary<string, Deudor>();
            using (ExcelPackage excelFile = new ExcelPackage(ExcelRepositoryConfiguration.FilePath))
            {
                int iRow = 4;
                ExcelWorksheet PrestamosWorksheet = excelFile.Workbook.Worksheets["Personas"];


                while (PrestamosWorksheet.Cells[iRow, 3].Value is not null)
                {
                    int keyId = Convert.ToInt32(PrestamosWorksheet.Cells[iRow, 3].Value);
                    DeudoresById.Add(keyId, new Deudor()
                    {
                        Id = keyId,
                        Nombre = PrestamosWorksheet.Cells[iRow, 4].Value.ToString(),
                        Parentezco = PrestamosWorksheet.Cells[iRow, 5].Value.ToString(),
                        Alias = PrestamosWorksheet.Cells[iRow, 6].Value.ToString(),
                    });

                    string keyAlias = PrestamosWorksheet.Cells[iRow, 6].Value.ToString();
                    DeudoresByAlias.Add(keyAlias, new Deudor()
                    {
                        Id = Convert.ToInt32(PrestamosWorksheet.Cells[iRow, 3].Value),
                        Nombre = PrestamosWorksheet.Cells[iRow, 4].Value.ToString(),
                        Parentezco = PrestamosWorksheet.Cells[iRow, 5].Value.ToString(),
                        Alias = keyAlias,
                    });

                    iRow++;
                }
            }
            _logger.LogInformation("Data Actualizada");
        }
    }
}
