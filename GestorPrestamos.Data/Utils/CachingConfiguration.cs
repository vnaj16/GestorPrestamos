using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Data.Utils
{
    public class CachingConfiguration
    {
        public static int LifetimeInSecondsForDeudoresDictionary { get; set; }
        public void SetLifetimeInSecondsForDeudoresDictionary(int seconds)
        {
            LifetimeInSecondsForDeudoresDictionary = seconds;
        }
    }
}
