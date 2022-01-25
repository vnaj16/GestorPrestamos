using GestorPrestamos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Domain.Responses
{
    public class RegisterPaymentLoanReceivableResponse
    {
        public Prestamo UpdatedLoan { get; set; }
        public bool RegisterSucceeded;
        public List<string> ErrorMessages { get; set; }

    }
}
