//using DocumentFormat.OpenXml.Office2010.Excel;
//using DocumentFormat.OpenXml.Spreadsheet;
using GestorPrestamos.Domain.Entities;
using GestorPrestamos.Domain.Interfaces.Repository;
using GestorPrestamos.Data.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace GestorPrestamos.Data.Implementations.Excel
{
    public class PrestamoExcelRepository : IPrestamoRepository
    {
        //TODO: Podria crear un extension method que usare en ASP.NET Core para configurar este servicio (cadena conexion, formato, etc)
        //https://stackoverflow.com/questions/27880433/using-iconfiguration-in-c-sharp-class-library/39548459#39548459
        private readonly DeudoresDictionary deudoresDictionary;
        public PrestamoExcelRepository()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            deudoresDictionary = new DeudoresDictionary();
        }
        public Prestamo Add(Prestamo newEntity)
        {
            using (var excelFile = new ExcelPackage(new FileInfo(ExcelRepositoryConfiguration.FilePath)))
            {
                newEntity.Id = Guid.NewGuid().ToString();
                int iRowToInsert = GetRowIndexToInsert(excelFile);
                ExcelWorksheet PrestamosWorksheet = excelFile.Workbook.Worksheets["Préstamos"];

                #region Register Data
                PrestamosWorksheet.Cells[iRowToInsert, 1].Value = newEntity.Id;
                PrestamosWorksheet.Cells[iRowToInsert, 2].Value = deudoresDictionary.GetDeudoresById()[newEntity.IdDeudor].Alias;
                PrestamosWorksheet.Cells[iRowToInsert, 3].Value = newEntity.MontoPrestado;
                PrestamosWorksheet.Cells[iRowToInsert, 4].Value = newEntity.FechaPrestamo;
                PrestamosWorksheet.Cells[iRowToInsert, 5].Value = newEntity.Descripcion;
                PrestamosWorksheet.Cells[iRowToInsert, 6].Value = newEntity.Comision;
                PrestamosWorksheet.Cells[iRowToInsert, 7].Value = newEntity.Intereses;
                PrestamosWorksheet.Cells[iRowToInsert, 8].Value = newEntity.DineroDevueltoParcial;
                PrestamosWorksheet.Cells[iRowToInsert, 9].Value = newEntity.MontoPorPagar;
                PrestamosWorksheet.Cells[iRowToInsert, 10].Value = newEntity.DeudaTotal;
                PrestamosWorksheet.Cells[iRowToInsert, 11].Value = newEntity.Estado;
                PrestamosWorksheet.Cells[iRowToInsert, 12].Value = newEntity.Notas;
                PrestamosWorksheet.Cells[iRowToInsert, 13].Value = newEntity.FechaPactadaDevolucion;
                #endregion

                UpdateRowIndexToInsert(excelFile);

                excelFile.Save();

                return newEntity;

            }
        }

        private int GetRowIndexToInsert(ExcelPackage excelFile)
        {
            int iRow = 2;
            int iColumn = 1;
            string parameterName = "RowIndexToInsert";
            int parameterValue = default(int);
            ExcelWorksheet VariablesWorksheet = excelFile.Workbook.Worksheets["Variables"];

            while (VariablesWorksheet.Cells[iRow, iColumn].Value is not null)
            {
                if ((VariablesWorksheet.Cells[iRow, iColumn].Value?.ToString() ?? String.Empty).Equals(parameterName))
                {

                    parameterValue = Convert.ToInt32(VariablesWorksheet.Cells[iRow, iColumn + 1].Value);
                    return parameterValue;
                }
                iRow++;
            }

            throw new Exception($"This parameter {parameterName} doesn't exist");
        }

        private void UpdateRowIndexToInsert(ExcelPackage excelFile)
        {
            int lastIndex = GetRowIndexToInsert(excelFile);
            int newIndex = lastIndex + 1;

            int iRow = 2;
            int iColumn = 1;
            string parameterName = "RowIndexToInsert";
            ExcelWorksheet VariablesWorksheet = excelFile.Workbook.Worksheets["Variables"];
            while (VariablesWorksheet.Cells[iRow, iColumn].Value is not null)
            {
                if ((VariablesWorksheet.Cells[iRow, iColumn].Value?.ToString() ?? String.Empty).Equals(parameterName))
                {
                    VariablesWorksheet.Cells[iRow, iColumn + 1].Value = newIndex;
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
            using (ExcelPackage excelFile = new ExcelPackage(ExcelRepositoryConfiguration.FilePath))
            {
                int iRow = 2;
                ExcelWorksheet PrestamosWorksheet = excelFile.Workbook.Worksheets["Préstamos"];

                List<Prestamo> list = new();

                //TODO: Para relacionar con Deudor, podria tener un Dictionary<Alias,Deudor> para obtener la data, similar como en NovoApp

                while (PrestamosWorksheet.Cells[iRow, 1].Value is not null)
                {
                    list.Add(new Prestamo()
                    {
                        Id = PrestamosWorksheet.Cells[iRow, 1].Value?.ToString() ?? String.Empty,
                        MontoPrestado = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 3].Value),
                        FechaPrestamo = Convert.ToDateTime(PrestamosWorksheet.Cells[iRow, 4].Value),
                        Descripcion = PrestamosWorksheet.Cells[iRow, 5].Value?.ToString() ?? String.Empty,
                        Comision = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 6].Value),
                        Intereses = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 7].Value),
                        DineroDevueltoParcial = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 8].Value),
                        Estado = PrestamosWorksheet.Cells[iRow, 11].Value?.ToString() ?? String.Empty,
                        Notas = PrestamosWorksheet.Cells[iRow, 12].Value?.ToString() ?? String.Empty,
                        FechaPactadaDevolucion = Convert.ToDateTime(PrestamosWorksheet.Cells[iRow, 13].Value)
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
            using (ExcelPackage excelFile = new ExcelPackage(ExcelRepositoryConfiguration.FilePath))
            {
                int iRow = 2;
                int iColumn = 1;
                ExcelWorksheet PrestamosWorksheet = excelFile.Workbook.Worksheets["Préstamos"];

                //TODO: Para relacionar con Deudor, podria tener un Dictionary<Alias,Deudor> para obtener la data, similar como en NovoApp

                while (PrestamosWorksheet.Cells[iRow, iColumn].Value is not null)
                {
                    if ((PrestamosWorksheet.Cells[iRow, iColumn].Value?.ToString() ?? String.Empty).Equals(id))
                    {
                        return new Prestamo()
                        {
                            Id = PrestamosWorksheet.Cells[iRow, 1].Value?.ToString() ?? String.Empty,
                            IdDeudor = deudoresDictionary.GetDeudoresByAlias()[PrestamosWorksheet.Cells[iRow, 2].Value.ToString()].Id,
                            MontoPrestado = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 3].Value),
                            FechaPrestamo = Convert.ToDateTime(PrestamosWorksheet.Cells[iRow, 4].Value),
                            Descripcion = PrestamosWorksheet.Cells[iRow, 5].Value?.ToString() ?? String.Empty,
                            Comision = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 6].Value),
                            Intereses = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 7].Value),
                            DineroDevueltoParcial = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 8].Value),
                            Estado = PrestamosWorksheet.Cells[iRow, 11].Value?.ToString() ?? String.Empty,
                            Notas = PrestamosWorksheet.Cells[iRow, 12].Value?.ToString() ?? String.Empty,
                            FechaPactadaDevolucion = Convert.ToDateTime(PrestamosWorksheet.Cells[iRow, 13].Value)
                        };
                    }

                    iRow++;
                }

                return null;
            }
        }

        public Prestamo GetByIdWithDeudorEntityIncluded(string id)
        {
            using (ExcelPackage excelFile = new ExcelPackage(ExcelRepositoryConfiguration.FilePath))
            {
                int iRow = 2;
                int iColumn = 1;
                ExcelWorksheet PrestamosWorksheet = excelFile.Workbook.Worksheets["Préstamos"];

                //TODO: Para relacionar con Deudor, podria tener un Dictionary<Alias,Deudor> para obtener la data, similar como en NovoApp

                while (PrestamosWorksheet.Cells[iRow, iColumn].Value is not null)
                {
                    if ((PrestamosWorksheet.Cells[iRow, iColumn].Value?.ToString() ?? String.Empty).Equals(id))
                    {
                        string aliasDeudor = PrestamosWorksheet.Cells[iRow, 2].Value?.ToString() ?? String.Empty;
                        return new Prestamo()
                        {
                            Id = PrestamosWorksheet.Cells[iRow, 1].Value?.ToString() ?? String.Empty,
                            MontoPrestado = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 3].Value),
                            FechaPrestamo = Convert.ToDateTime(PrestamosWorksheet.Cells[iRow, 4].Value),
                            Descripcion = PrestamosWorksheet.Cells[iRow, 5].Value?.ToString() ?? String.Empty,
                            Comision = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 6].Value),
                            Intereses = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 7].Value),
                            DineroDevueltoParcial = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 8].Value),
                            Estado = PrestamosWorksheet.Cells[iRow, 11].Value?.ToString() ?? String.Empty,
                            Notas = PrestamosWorksheet.Cells[iRow, 12].Value?.ToString() ?? String.Empty,
                            FechaPactadaDevolucion = Convert.ToDateTime(PrestamosWorksheet.Cells[iRow, 13].Value),

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
            using (ExcelPackage excelFile = new ExcelPackage(ExcelRepositoryConfiguration.FilePath))
            {
                int iRowToUpdate = 2;
                int iColumn = 1;
                ExcelWorksheet PrestamosWorksheet = excelFile.Workbook.Worksheets["Préstamos"];

                //TODO: Para relacionar con Deudor, podria tener un Dictionary<Alias,Deudor> para obtener la data, similar como en NovoApp

                while (!string.IsNullOrEmpty(PrestamosWorksheet.Cells[iRowToUpdate, iColumn].Value.ToString()))
                {
                    if (PrestamosWorksheet.Cells[iRowToUpdate, iColumn].Value.ToString().Equals(updatedEntity.Id))
                    {
                        break;
                    }

                    iRowToUpdate++;
                }

                #region Update Data
                PrestamosWorksheet.Cells[iRowToUpdate, 3].Value = updatedEntity.MontoPrestado;
                PrestamosWorksheet.Cells[iRowToUpdate, 4].Value = updatedEntity.FechaPrestamo;
                PrestamosWorksheet.Cells[iRowToUpdate, 5].Value = updatedEntity.Descripcion;
                PrestamosWorksheet.Cells[iRowToUpdate, 6].Value = updatedEntity.Comision;
                PrestamosWorksheet.Cells[iRowToUpdate, 7].Value = updatedEntity.Intereses;
                PrestamosWorksheet.Cells[iRowToUpdate, 8].Value = updatedEntity.DineroDevueltoParcial;
                PrestamosWorksheet.Cells[iRowToUpdate, 9].Value = updatedEntity.MontoPorPagar;
                PrestamosWorksheet.Cells[iRowToUpdate, 10].Value = updatedEntity.DeudaTotal;
                PrestamosWorksheet.Cells[iRowToUpdate, 11].Value = updatedEntity.Estado;
                PrestamosWorksheet.Cells[iRowToUpdate, 12].Value = updatedEntity.Notas;
                PrestamosWorksheet.Cells[iRowToUpdate, 13].Value = updatedEntity.FechaPactadaDevolucion;
                #endregion

                excelFile.Save();

                return updatedEntity;
            }
        }

        public List<Prestamo> GetAllWithStatusToPay()
        {
            using (ExcelPackage excelFile = new ExcelPackage(ExcelRepositoryConfiguration.FilePath))
            {
                int iRow = 2;
                ExcelWorksheet PrestamosWorksheet = excelFile.Workbook.Worksheets["Préstamos"];

                List<Prestamo> list = new();

                //TODO: Para relacionar con Deudor, podria tener un Dictionary<Alias,Deudor> para obtener la data, similar como en NovoApp

                while (PrestamosWorksheet.Cells[iRow, 1].Value is not null)
                {
                    if (PrestamosWorksheet.Cells[iRow, 11].Value.ToString() == "Por Pagar")
                    {
                        string aliasDeudor = PrestamosWorksheet.Cells[iRow, 2].Value?.ToString() ?? String.Empty;
                        list.Add(new Prestamo()
                        {
                            Id = PrestamosWorksheet.Cells[iRow, 1].Value?.ToString() ?? String.Empty,
                            MontoPrestado = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 3].Value ?? 0),
                            FechaPrestamo = Convert.ToDateTime(PrestamosWorksheet.Cells[iRow, 4].Value),
                            Descripcion = PrestamosWorksheet.Cells[iRow, 5].Value?.ToString() ?? String.Empty,
                            Comision = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 6].Value),
                            Intereses = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 7].Value),
                            DineroDevueltoParcial = Convert.ToSingle(PrestamosWorksheet.Cells[iRow, 8].Value),
                            Estado = "Por Pagar",
                            Notas = PrestamosWorksheet.Cells[iRow, 12].Value?.ToString() ?? String.Empty,
                            FechaPactadaDevolucion = Convert.ToDateTime(PrestamosWorksheet.Cells[iRow, 13].Value),
                            IdDeudor = deudoresDictionary.GetDeudoresByAlias()[aliasDeudor].Id,
                            Deudor = deudoresDictionary.GetDeudoresByAlias()[aliasDeudor]
                        });
                    }

                    iRow++;
                }

                return list;
            }
        }
    }
}
