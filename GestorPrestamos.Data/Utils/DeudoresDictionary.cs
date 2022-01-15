using GestorPrestamos.Data.Entities;
using GestorPrestamos.Data.Implementations.Excel;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Data.Utils
{
    public class DeudoresDictionary
    {
        public Dictionary<int, Deudor> DeudoresById { get; set; }
        public Dictionary<string, Deudor> DeudoresByAlias { get; set; }

        public DeudoresDictionary()
        {
            InitializeDeudoresById();
            InitializeDeudoresByAlias();
        }

        private void InitializeDeudoresById()
        {
            DeudoresById = new Dictionary<int, Deudor>();
            using (SLDocument excelFile = new SLDocument(ExcelRepositoryConfiguration.FilePath))
            {
                int iRow = 4;
                excelFile.SelectWorksheet("Personas");
                //List<Prestamo> list = new();

                while (!string.IsNullOrEmpty(excelFile.GetCellValueAsString(iRow, 3)))
                {
                    int key = excelFile.GetCellValueAsInt32(iRow, 3);
                    DeudoresById.Add(key, new Deudor()
                    {
                        Id = key,
                        Nombre = excelFile.GetCellValueAsString(iRow, 4),
                        Parentezco = excelFile.GetCellValueAsString(iRow, 5),
                        Alias = excelFile.GetCellValueAsString(iRow, 6)
                    });

                    iRow++;
                }
            }
        }

        private void InitializeDeudoresByAlias()
        {
            DeudoresByAlias = new Dictionary<string, Deudor>();
            using (SLDocument excelFile = new SLDocument(ExcelRepositoryConfiguration.FilePath))
            {
                int iRow = 4;
                excelFile.SelectWorksheet("Personas");
                //List<Prestamo> list = new();

                while (!string.IsNullOrEmpty(excelFile.GetCellValueAsString(iRow, 3)))
                {
                    string key = excelFile.GetCellValueAsString(iRow, 6);
                    DeudoresByAlias.Add(key, new Deudor()
                    {
                        Alias = key,
                        Id = excelFile.GetCellValueAsInt32(iRow, 3),
                        Nombre = excelFile.GetCellValueAsString(iRow, 4),
                        Parentezco = excelFile.GetCellValueAsString(iRow, 5)
                    });

                    iRow++;
                }

            }
        }
    }
}
