using DocumentFormat.OpenXml.Office2010.Excel;
using GestorPrestamos.Data.Entities;
using GestorPrestamos.Data.Interfaces;
using GestorPrestamos.Data.Utils;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Data.Implementations.Excel
{
    public class PrestamoExcelRepository : IPrestamoRepository
    {
        //TODO: Podria crear un extension method que usare en ASP.NET Core para configurar este servicio (cadena conexion, formato, etc)
        //https://stackoverflow.com/questions/27880433/using-iconfiguration-in-c-sharp-class-library/39548459#39548459
        private readonly DeudoresDictionary deudoresDictionary;
        public PrestamoExcelRepository()
        {
            deudoresDictionary = new DeudoresDictionary();
        }
        public Prestamo Add(Prestamo newEntity)
        {
            //TODO: Tendria que buscar el iRow donde puedo escribir (es decir donde esté vacio) y ahi agregar el nuevo objeto
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {//NO SE IMPLEMENTARÁ
            throw new NotImplementedException();
        }

        public List<Prestamo> GetAll()
        {
            using (SLDocument excelFile = new SLDocument(ExcelRepositoryConfiguration.FilePath))
            {
                int iRow = 2;
                excelFile.SelectWorksheet("Préstamos");
                List<Prestamo> list = new();

                //TODO: Para relacionar con Deudor, podria tener un Dictionary<Alias,Deudor> para obtener la data, similar como en NovoApp

                while (!string.IsNullOrEmpty(excelFile.GetCellValueAsString(iRow, 1)))
                {
                    list.Add(new Prestamo()
                    {
                        Id = excelFile.GetCellValueAsString(iRow, 1),
                        MontoPrestado = Convert.ToSingle(excelFile.GetCellValueAsDouble(iRow, 3)),
                        FechaPrestamo = excelFile.GetCellValueAsDateTime(iRow, 4),
                        Descripcion = excelFile.GetCellValueAsString(iRow,5),
                        Comision = Convert.ToSingle(excelFile.GetCellValueAsDouble(iRow, 6)),
                        Intereses = Convert.ToSingle(excelFile.GetCellValueAsDouble(iRow, 7)),
                        DineroDevueltoParcial = Convert.ToSingle(excelFile.GetCellValueAsDouble(iRow, 8)),
                        Estado = excelFile.GetCellValueAsString(iRow, 11),
                        Notas = excelFile.GetCellValueAsString(iRow, 12),
                        FechaPactadaDevolucion = excelFile.GetCellValueAsDateTime(iRow, 13)
                    });

                    iRow++;
                }

                return list;
            }
        }

        public List<Prestamo> GetAllWithDeudorEntityIncluded()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prestamo entity if is found, Null if is not Found
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Prestamo entity if is found, Null if is not Found</returns>
        public Prestamo GetById(string id)
        {
            using (SLDocument excelFile = new SLDocument(ExcelRepositoryConfiguration.FilePath))
            {
                int iRow = 2;
                int iColumn = 1;
                excelFile.SelectWorksheet("Préstamos");

                //TODO: Para relacionar con Deudor, podria tener un Dictionary<Alias,Deudor> para obtener la data, similar como en NovoApp

                while (!string.IsNullOrEmpty(excelFile.GetCellValueAsString(iRow, iColumn)))
                {
                    if (excelFile.GetCellValueAsString(iRow, iColumn).Equals(id))
                    {
                        return new Prestamo()
                        {
                            Id = excelFile.GetCellValueAsString(iRow, 1),
                            MontoPrestado = Convert.ToSingle(excelFile.GetCellValueAsDouble(iRow, 3)),
                            FechaPrestamo = excelFile.GetCellValueAsDateTime(iRow, 4),
                            Descripcion = excelFile.GetCellValueAsString(iRow, 5),
                            Comision = Convert.ToSingle(excelFile.GetCellValueAsDouble(iRow, 6)),
                            Intereses = Convert.ToSingle(excelFile.GetCellValueAsDouble(iRow, 7)),
                            DineroDevueltoParcial = Convert.ToSingle(excelFile.GetCellValueAsDouble(iRow, 8)),
                            Estado = excelFile.GetCellValueAsString(iRow, 11),
                            Notas = excelFile.GetCellValueAsString(iRow, 12),
                            FechaPactadaDevolucion = excelFile.GetCellValueAsDateTime(iRow, 13)
                        };
                    }

                    iRow++;
                }

                return null;
            }
        }

        public Prestamo GetByIdWithDeudorEntityIncluded(string id)
        {
            using (SLDocument excelFile = new SLDocument(ExcelRepositoryConfiguration.FilePath))
            {
                int iRow = 2;
                int iColumn = 1;
                excelFile.SelectWorksheet("Préstamos");

                //TODO: Para relacionar con Deudor, podria tener un Dictionary<Alias,Deudor> para obtener la data, similar como en NovoApp

                while (!string.IsNullOrEmpty(excelFile.GetCellValueAsString(iRow, iColumn)))
{
                    if (excelFile.GetCellValueAsString(iRow, iColumn).Equals(id))
                    {
                        string aliasDeudor = excelFile.GetCellValueAsString(iRow, 2);
                        return new Prestamo()
                        {
                            Id = excelFile.GetCellValueAsString(iRow, 1),
                            MontoPrestado = Convert.ToSingle(excelFile.GetCellValueAsDouble(iRow, 3)),
                            FechaPrestamo = excelFile.GetCellValueAsDateTime(iRow, 4),
                            Descripcion = excelFile.GetCellValueAsString(iRow, 5),
                            Comision = Convert.ToSingle(excelFile.GetCellValueAsDouble(iRow, 6)),
                            Intereses = Convert.ToSingle(excelFile.GetCellValueAsDouble(iRow, 7)),
                            DineroDevueltoParcial = Convert.ToSingle(excelFile.GetCellValueAsDouble(iRow, 8)),
                            Estado = excelFile.GetCellValueAsString(iRow, 11),
                            Notas = excelFile.GetCellValueAsString(iRow, 12),
                            FechaPactadaDevolucion = excelFile.GetCellValueAsDateTime(iRow, 13),
                            IdDeudor = deudoresDictionary.DeudoresByAlias[aliasDeudor].Id,
                            Deudor = deudoresDictionary.DeudoresByAlias[aliasDeudor]
                        };
                    }

                    iRow++;
                }

                return null;
            }
        }

        public Prestamo Update(Prestamo updatedEntity)
        { //TODO: Tendria que buscar el iRow de dicha entidad y actualizar los campos respectivos (lo contratio a cuando leo)
            throw new NotImplementedException();
        }
    }
}
