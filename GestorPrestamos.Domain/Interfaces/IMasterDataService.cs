using GestorPrestamos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Domain.Interfaces
{
    public interface IMasterDataService
    {
        public Dictionary<int, Deudor> GetDebtors();
    }
}
