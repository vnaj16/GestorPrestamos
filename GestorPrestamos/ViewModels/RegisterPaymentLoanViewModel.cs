using GestorPrestamos.Domain.Utils;

namespace GestorPrestamos.ViewModels
{
    public class RegisterPaymentLoanViewModel
    {
        public string Id { get; set; }
        public PaymentType PaymentType { get; set; }
        public float AmountPaid { get; set; }
    }
}
