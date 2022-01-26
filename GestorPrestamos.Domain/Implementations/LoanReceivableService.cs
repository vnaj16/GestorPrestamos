﻿using GestorPrestamos.Domain.Entities;
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

            var loan = _prestamoRepository.GetById(paymentLoanReceivable.LoanId);
            if (loan is null)
            {
                throw new Exception($"Loan with id {paymentLoanReceivable.LoanId} doesn't exist");
            }
            //TODO: Validar que el tipo de pago concuerde ocn la cantidad ingresada

            switch (paymentLoanReceivable.PaymentType)
            {//TODO: Incluso creo que podria quitar el tipo de pago a seleccionar, eso se podria inferir
                case Utils.PaymentType.PagoParcial:
                    loan.DineroDevueltoParcial += paymentLoanReceivable.AmountPaid;
                    loan.Estado = "Por Pagar";
                    loan.Notas += $"El dia {DateTime.Now.ToString("d")} se realizó un pago parcial de {paymentLoanReceivable.AmountPaid} soles | ";
                    if (loan.MontoPorPagar < 0)
                    {
                        loan.Estado = "Pagado";
                        loan.Notas += $"El dia {DateTime.Now.ToString("d")} se terminó de pagar este préstam | ";
                    }
                    break;
                case Utils.PaymentType.PagoTotal:
                    loan.DineroDevueltoParcial = 0;
                    loan.Estado = "Pagado";
                    loan.Notas += $"El dia {DateTime.Now.ToString("d")} se pagó totalmente este prestamo | ";
                    break;
                default:
                    throw new Exception("PaymentType not supported: " + paymentLoanReceivable.PaymentType.ToString());
                    break;
            }

            _prestamoRepository.Update(loan);

            return new RegisterPaymentLoanReceivableResponse()
            {
                UpdatedLoan = loan,
                RegisterSucceeded = true
            };
        }
    }
}
