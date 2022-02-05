using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Domain.Entities
{
    public class Prestamo
    {
        public string Id { get; set; }
        public float MontoPrestado { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaPrestamo { get; set; } = DateTime.Now;
        public float Comision { get; set; }
        public float Intereses { get; set; }
        public float DineroDevueltoParcial { get; set; }
        public float MontoPorPagar => MontoPrestado + Comision + Intereses - DineroDevueltoParcial;
        
        public float DeudaTotal => MontoPorPagar + DineroDevueltoParcial;

        public string Estado { get; set; } = "Por Pagar";
        public string Notas { get; set; }
        public DateTime FechaPactadaDevolucion { get; set; }
        public int IdDeudor { get; set; }
        public Deudor Deudor { get; set; }
    }
}
