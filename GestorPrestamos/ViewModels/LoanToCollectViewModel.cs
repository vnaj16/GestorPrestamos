using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;

namespace GestorPrestamos.ViewModels
{
    public class LoanToCollectViewModel
    {
        public string Id { get; set; }
        public float BorrowedAmount { get; set; }
        public string Debtor { get; set; }
        public DateTime LoanDate { get; set; }
        public string Description { get; set; }
        public float Commission { get; set; }
        public float Interest { get; set; }
        public float PartialRefund { get; set; }
        public float AmountToPay { get; set; }
        public float TotalDebt { get; set; }
    }
}
