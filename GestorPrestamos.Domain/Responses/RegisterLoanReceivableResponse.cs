using GestorPrestamos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Domain.Responses
{
    public class RegisterLoanReceivableResponse
    {
        public Prestamo RegisteredEntity;
        public bool RegisterSucceeded;
    }
}
