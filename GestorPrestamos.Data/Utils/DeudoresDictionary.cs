using GestorPrestamos.Domain.Entities;
using GestorPrestamos.Data.Implementations.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorPrestamos.Domain.MasterData;
using OfficeOpenXml;

namespace GestorPrestamos.Data.Utils
{
    //CREA OTRA CLASE, QUE TENGA LA PALABRA CACHEABLE
    public class DeudoresDictionary : IDeudoresDictionary
    {
        private Dictionary<int, Deudor> DeudoresById { get; set; }
        private Dictionary<string, Deudor> DeudoresByAlias { get; set; }

        public DeudoresDictionary()
        {
            InitializeDeudoresById();
            InitializeDeudoresByAlias();
        }

        private void InitializeDeudoresById()
        {
            DeudoresById = new Dictionary<int, Deudor>();
            using (ExcelPackage excelFile = new ExcelPackage(ExcelRepositoryConfiguration.FilePath))
            {
                int iRow = 4;
                ExcelWorksheet PrestamosWorksheet = excelFile.Workbook.Worksheets["Personas"];
                //List<Prestamo> list = new();

                while (PrestamosWorksheet.Cells[iRow, 3].Value is not null)
                {
                    int key = Convert.ToInt32(PrestamosWorksheet.Cells[iRow, 3].Value);
                    DeudoresById.Add(key, new Deudor()
                    {
                        Id = key,
                        Nombre = PrestamosWorksheet.Cells[iRow, 4].Value.ToString(),
                        Parentezco = PrestamosWorksheet.Cells[iRow, 5].Value.ToString(),
                        Alias = PrestamosWorksheet.Cells[iRow,6].Value.ToString(),
                    });

                    iRow++;
                }
            }
        }

        private void InitializeDeudoresByAlias()
        {
            DeudoresByAlias = new Dictionary<string, Deudor>();
            using (ExcelPackage excelFile = new ExcelPackage(ExcelRepositoryConfiguration.FilePath))
            {
                int iRow = 4;
                ExcelWorksheet PrestamosWorksheet = excelFile.Workbook.Worksheets["Personas"];
                //List<Prestamo> list = new();

                while (PrestamosWorksheet.Cells[iRow, 3].Value is not null)
                {
                    string key = PrestamosWorksheet.Cells[iRow, 6].Value.ToString();
                    DeudoresByAlias.Add(key, new Deudor()
                    {
                        Id = Convert.ToInt32(PrestamosWorksheet.Cells[iRow, 3].Value),
                        Nombre = PrestamosWorksheet.Cells[iRow, 4].Value.ToString(),
                        Parentezco = PrestamosWorksheet.Cells[iRow, 5].Value.ToString(),
                        Alias = key,
                    });

                    iRow++;
                }
            }
        }

        public Dictionary<int, Deudor> GetDeudoresById()
        {
            return DeudoresById;
        }

        public Dictionary<string, Deudor> GetDeudoresByAlias()
        {
            return DeudoresByAlias;
        }
    }
}
