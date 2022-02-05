using GestorPrestamos.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Domain.Entities
{
    public class PaymentLoanReceivable
    {
        //public Prestamo Prestamo { get; set; }
        public string LoanId { get; set; }
        public PaymentType PaymentType { get; set; }
        public float AmountPaid { get; set; }
    }
}
