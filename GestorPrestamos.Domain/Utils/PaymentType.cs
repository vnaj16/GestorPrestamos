using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GestorPrestamos.Domain.Utils
{
    public enum PaymentType
    {
        [Display(Name = "Pago Parcial")]
        PagoParcial,
        [Display(Name = "Pago Total")]
        PagoTotal
    }
}
