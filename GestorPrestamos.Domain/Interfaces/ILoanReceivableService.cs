using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorPrestamos.Domain.Entities;
using GestorPrestamos.Domain.Responses;

namespace GestorPrestamos.Domain.Interfaces
{
    public interface ILoanReceivableService
    {
        public RegisterLoanReceivableResponse RegisterLoanReceivable(Prestamo prestamo);
        public List<Prestamo> GetAllLoanReceivable();
        //TODO: AGREGAR GET ALL PARA PROBAR ARQUITECTURA
        public Prestamo GetLoanReceivableById(string id);
        public RegisterPaymentLoanReceivableResponse RegisterPaymentLoanReceivable(PaymentLoanReceivable paymentLoanReceivable);
    }
}
