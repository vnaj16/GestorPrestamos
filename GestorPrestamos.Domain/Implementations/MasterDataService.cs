using GestorPrestamos.Domain.Entities;
using GestorPrestamos.Domain.Interfaces;
using GestorPrestamos.Domain.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Domain.Implementations
{
    public class MasterDataService : IMasterDataService
    {
        private readonly IDeudoresDictionary _deudoresDictionary;

        public MasterDataService(IDeudoresDictionary deudoresDictionary)
        {
            _deudoresDictionary = deudoresDictionary;
        }

        public Dictionary<int, Deudor> GetDebtors()
        {
            return _deudoresDictionary.GetDeudoresById();
        }
    }
}
