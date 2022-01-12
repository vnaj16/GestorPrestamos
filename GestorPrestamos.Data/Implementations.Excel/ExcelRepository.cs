using GestorPrestamos.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Data.Implementations.Excel
{
    public class ExcelRepository : IRepository
    {
        //TODO: Podria crear un extension method que usare en ASP.NET Core para configurar este servicio (cadena conexion, formato, etc)
        //https://stackoverflow.com/questions/27880433/using-iconfiguration-in-c-sharp-class-library/39548459#39548459
        public int GetData()
        {
            return 146;
        }
    }
}
