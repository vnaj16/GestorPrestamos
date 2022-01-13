using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Data.Entities
{
    public class Deudor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Parentezco { get; set; }
        public string Alias { get; set; }
    }
}
