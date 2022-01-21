using GestorPrestamos.Data.Implementations.Excel;
using GestorPrestamos.Domain.Entities;
using GestorPrestamos.Domain.MasterData;
using GestorPrestamos.Domain.Utils;
using Microsoft.Extensions.Logging;
using SpreadsheetLight;
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
            using (SLDocument excelFile = new SLDocument(ExcelRepositoryConfiguration.FilePath))
            {
                int iRow = 4;
                excelFile.SelectWorksheet("Personas");

                while (!string.IsNullOrEmpty(excelFile.GetCellValueAsString(iRow, 3)))
                {
                    int keyId = excelFile.GetCellValueAsInt32(iRow, 3);
                    DeudoresById.Add(keyId, new Deudor()
                    {
                        Id = keyId,
                        Nombre = excelFile.GetCellValueAsString(iRow, 4),
                        Parentezco = excelFile.GetCellValueAsString(iRow, 5),
                        Alias = excelFile.GetCellValueAsString(iRow, 6)
                    });

                    string keyAlias = excelFile.GetCellValueAsString(iRow, 6);
                    DeudoresByAlias.Add(keyAlias, new Deudor()
                    {
                        Alias = keyAlias,
                        Id = excelFile.GetCellValueAsInt32(iRow, 3),
                        Nombre = excelFile.GetCellValueAsString(iRow, 4),
                        Parentezco = excelFile.GetCellValueAsString(iRow, 5)
                    });

                    iRow++;
                }
            }
            _logger.LogInformation("Data Actualizada");
        }
    }
}
