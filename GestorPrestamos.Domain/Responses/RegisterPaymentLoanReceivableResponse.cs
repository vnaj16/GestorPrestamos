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
        public RegisterPaymentLoanReceivableResponse()
        {
            ErrorMessages = new List<string>();
        }
        public Prestamo UpdatedLoan { get; set; }
        public bool RegisterSucceeded { get; set; }
        public List<string> ErrorMessages { get; set; }

    }
}
