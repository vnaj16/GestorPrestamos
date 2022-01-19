using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using GestorPrestamos.Domain.Entities;
using GestorPrestamos.Domain.Interfaces.Repository;
using GestorPrestamos.Data.Utils;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
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
            using (SLDocument excelFile = new SLDocument(ExcelRepositoryConfiguration.FilePath))
            {
                newEntity.Id = Guid.NewGuid().ToString();

                int iRowToInsert = GetRowIndexToInsert(excelFile);
                excelFile.SelectWorksheet("Préstamos");

                #region Register Data

                excelFile.SetCellValue(iRowToInsert, 1, newEntity.Id);
                excelFile.SetCellValue(iRowToInsert, 2, deudoresDictionary.GetDeudoresById()[newEntity.IdDeudor].Alias);
                excelFile.SetCellValue(iRowToInsert, 3, newEntity.MontoPrestado);
                excelFile.SetCellValue(iRowToInsert, 4, newEntity.FechaPrestamo);
                excelFile.SetCellValue(iRowToInsert, 5, newEntity.Descripcion);
                excelFile.SetCellValue(iRowToInsert, 6, newEntity.Comision);
                excelFile.SetCellValue(iRowToInsert, 7, newEntity.Intereses);
                excelFile.SetCellValue(iRowToInsert, 8, newEntity.DineroDevueltoParcial);
                excelFile.SetCellValue(iRowToInsert, 9, newEntity.MontoPorPagar);
                excelFile.SetCellValue(iRowToInsert, 10, newEntity.DeudaTotal);
                excelFile.SetCellValue(iRowToInsert, 11, newEntity.Estado);
                excelFile.SetCellValue(iRowToInsert, 12, newEntity.Notas);
                excelFile.SetCellValue(iRowToInsert, 13, newEntity.FechaPactadaDevolucion);
                #endregion

                UpdateRowIndexToInsert(excelFile);

                excelFile.Save();

                return newEntity;
            }
        }

        private int GetRowIndexToInsert(SLDocument excelFile)
        {
            int iRow = 2;
            int iColumn = 1;
            string parameterName = "RowIndexToInsert";
            int parameterValue = default(int);
            excelFile.SelectWorksheet("Variables");

            while (!string.IsNullOrEmpty(excelFile.GetCellValueAsString(iRow, iColumn)))
            {
                if (excelFile.GetCellValueAsString(iRow, iColumn).Equals(parameterName))
                {
                    parameterValue = excelFile.GetCellValueAsInt32(iRow, iColumn + 1);
                    return parameterValue;
                }
                iRow++;
            }

            throw new Exception($"This parameter {parameterName} doesn't exist");
        }

        private void UpdateRowIndexToInsert(SLDocument excelFile)
        {
            int lastIndex = GetRowIndexToInsert(excelFile);
            int newIndex = lastIndex + 1;

            int iRow = 2;
            int iColumn = 1;
            string parameterName = "RowIndexToInsert";
            excelFile.SelectWorksheet("Variables");
            while (!string.IsNullOrEmpty(excelFile.GetCellValueAsString(iRow, iColumn)))
            {
                if (excelFile.GetCellValueAsString(iRow, iColumn).Equals(parameterName))
                {
                    excelFile.SetCellValue(iRow, iColumn + 1, newIndex);
                    return;
                }
                iRow++;
            }
            throw new Exception($"This parameter {parameterName} doesn't exist");
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
                        Descripcion = excelFile.GetCellValueAsString(iRow, 5),
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
                            IdDeudor = deudoresDictionary.GetDeudoresByAlias()[aliasDeudor].Id,
                            Deudor = deudoresDictionary.GetDeudoresByAlias()[aliasDeudor]
                        };
                    }

                    iRow++;
                }

                return null;
            }
        }

        public Prestamo Update(Prestamo updatedEntity)
        { //TODO: Tendria que buscar el iRow de dicha entidad y actualizar los campos respectivos (lo contratio a cuando leo)
            using (SLDocument excelFile = new SLDocument(ExcelRepositoryConfiguration.FilePath))
            {
                int iRowToUpdate = 2;
                int iColumn = 1;
                excelFile.SelectWorksheet("Préstamos");

                //TODO: Para relacionar con Deudor, podria tener un Dictionary<Alias,Deudor> para obtener la data, similar como en NovoApp

                while (!string.IsNullOrEmpty(excelFile.GetCellValueAsString(iRowToUpdate, iColumn)))
                {
                    if (excelFile.GetCellValueAsString(iRowToUpdate, iColumn).Equals(updatedEntity.Id))
                    {
                        break;
                    }

                    iRowToUpdate++;
                }

                #region Update Data

                excelFile.SetCellValue(iRowToUpdate, 3, updatedEntity.MontoPrestado);
                excelFile.SetCellValue(iRowToUpdate, 4, updatedEntity.FechaPrestamo);
                excelFile.SetCellValue(iRowToUpdate, 5, updatedEntity.Descripcion);
                excelFile.SetCellValue(iRowToUpdate, 6, updatedEntity.Comision);
                excelFile.SetCellValue(iRowToUpdate, 7, updatedEntity.Intereses);
                excelFile.SetCellValue(iRowToUpdate, 8, updatedEntity.DineroDevueltoParcial);
                excelFile.SetCellValue(iRowToUpdate, 9, updatedEntity.MontoPorPagar);
                excelFile.SetCellValue(iRowToUpdate, 10, updatedEntity.DeudaTotal);
                excelFile.SetCellValue(iRowToUpdate, 11, updatedEntity.Estado);
                excelFile.SetCellValue(iRowToUpdate, 12, updatedEntity.Notas);
                excelFile.SetCellValue(iRowToUpdate, 13, updatedEntity.FechaPactadaDevolucion);
                #endregion

                excelFile.Save();

                return updatedEntity;
            }
        }
    }
}
