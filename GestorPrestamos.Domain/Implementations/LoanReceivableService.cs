using GestorPrestamos.Domain.Entities;
using GestorPrestamos.Domain.Interfaces;
using GestorPrestamos.Domain.Interfaces.Repository;
using GestorPrestamos.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Domain.Implementations
{
    public class LoanReceivableService : ILoanReceivableService
    {
        private readonly IPrestamoRepository _prestamoRepository;

        public LoanReceivableService(IPrestamoRepository prestamoRepository)
        {
            _prestamoRepository = prestamoRepository;
        }

        public List<Prestamo> GetAllLoanReceivable()
        {
           return _prestamoRepository.GetAll();
        }

        public Prestamo GetLoanReceivableById(string id)
        {
           return _prestamoRepository.GetById(id);
        }

        public RegisterLoanReceivableResponse RegisterLoanReceivable(Prestamo prestamo)
        {
            //Here will be validation logic and Business Rules

            var entity = _prestamoRepository.Add(prestamo);
            return new RegisterLoanReceivableResponse()
            {
                RegisteredEntity = entity,
                RegisterSucceeded = true
            };
        }

        public RegisterPaymentLoanReceivableResponse RegisterPaymentLoanReceivable(PaymentLoanReceivable paymentLoanReceivable)
        {//TODO Aplicaria la l´´ogica

            //TODO: Traer el Prestamo de la base de datos, si no existe, lanzar error
            switch (paymentLoanReceivable.PaymentType)
            {
                case Utils.PaymentType.PagoParcial:

                    break;
                case Utils.PaymentType.PagoTotal:

                    break;
                default:
                    throw new Exception("PaymentType not supported: " + paymentLoanReceivable.PaymentType.ToString());
                    break;
            }
            return new RegisterPaymentLoanReceivableResponse()
            {
                
            };
        }
    }
}
