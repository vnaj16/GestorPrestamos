using GestorPrestamos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Domain.MasterData
{
    public interface IDeudoresDictionary
    {
        public Dictionary<int, Deudor> GetDeudoresById();
        public Dictionary<string, Deudor> GetDeudoresByAlias();
    }
}
