using GestorPrestamos.Domain.Entities;
using GestorPrestamos.Domain.Interfaces;
using GestorPrestamos.Domain.Interfaces.Repository;
using GestorPrestamos.Domain.Responses;
using GestorPrestamos.Domain.Utils;
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

        public List<Prestamo> GetAllLoanReceivableWithStatusToPay()
        {
            return _prestamoRepository.GetAllWithStatusToPay().OrderByDescending(l=>l.FechaPrestamo).ToList();
        }

        public Prestamo GetLoanReceivableById(string id)
        {
           return _prestamoRepository.GetById(id);
        }

        public StatsLoanToCollect GetStats()
        {
            var loanList = GetAllLoanReceivable();
            StatsLoanToCollect result = new StatsLoanToCollect()
            {
                NumberOfCollectedLoans = loanList.Where(l=>l.Estado == "Pagada").Count(),
                NumberOfLoansToCollect = loanList.Where(l => l.Estado == "Por Pagar").Count(),
                TotalAmountToBeCollected = loanList.Where(l => l.Estado == "Por Pagar").Sum(x => x.MontoPorPagar)
            };
            return result;  
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
            var response = new RegisterPaymentLoanReceivableResponse();

            var loan = _prestamoRepository.GetById(paymentLoanReceivable.LoanId);
            if (loan is null)
            {
                response.ErrorMessages.Add($"Loan with id {paymentLoanReceivable.LoanId} doesn't exist");
            }
            if (paymentLoanReceivable.AmountPaid > loan.DeudaTotal)
            {
                response.ErrorMessages.Add($"Se ingresó un monto mayor al dinero que se debe pagar: Monto Pagado: {paymentLoanReceivable.AmountPaid}, Monto que se debe pagar: {loan.DeudaTotal}");
            }
            //TODO: Validar que el tipo de pago concuerde ocn la cantidad ingresada

            switch (paymentLoanReceivable.PaymentType)
            {//TODO: Incluso creo que podria quitar el tipo de pago a seleccionar, eso se podria inferir
                case Utils.PaymentType.PagoParcial:
                    loan.DineroDevueltoParcial += paymentLoanReceivable.AmountPaid;
                    loan.Estado = "Por Pagar";
                    loan.Notas += $"El dia {DateTime.Now.ToString("d")} se realizó un pago parcial de {paymentLoanReceivable.AmountPaid} soles | ";
                    if (loan.MontoPorPagar == 0)
                    {
                        loan.Estado = "Pagado";
                        loan.Notas += $"El dia {DateTime.Now.ToString("d")} se terminó de pagar este préstam | ";
                    }
                    else if(loan.MontoPorPagar < 0)
                    {
                        response.ErrorMessages.Add(
                            $"Se ingresó un monto mayor al dinero que se debe pagar: Monto Pagado: {paymentLoanReceivable.AmountPaid}, Monto que queda por pagar: {loan.MontoPorPagar}"
                            );
                        break;
                    }
                    else
                    {

                    }
                    break;
                case Utils.PaymentType.PagoTotal:
                    loan.DineroDevueltoParcial = 0;
                    loan.Estado = "Pagado";
                    loan.Notas += $"El dia {DateTime.Now.ToString("d")} se pagó totalmente este prestamo | ";
                    break;
                default:
                    response.RegisterSucceeded = false;
                    response.ErrorMessages.Add(
                        "PaymentType not supported: " + paymentLoanReceivable.PaymentType.ToString()
                        );
                    break;
            }

            response.UpdatedLoan = _prestamoRepository.Update(loan);
            response.RegisterSucceeded = true;

            return response;
        }
    }
}
