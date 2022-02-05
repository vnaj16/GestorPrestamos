using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Domain.Utils
{
    public interface ICacheable
    {
        public int LifetimeInSeconds { get; set; }
        public void RefreshData();
    }
}
