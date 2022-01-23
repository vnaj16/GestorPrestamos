using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;

namespace GestorPrestamos.ViewModels
{
    public class InfoPrestamoViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Monto Prestado (S/.)")]
        public float BorrowedAmount { get; set; }

        [Display(Name = "Deudor")]
        public string Debtor { get; set; }

        [Display(Name = "Comisión (S/.)")]
        public float Commission { get; set; }

        [Display(Name = "Interés (S/.)")]
        public float Interest { get; set; }
        [Display(Name = "Deuda Total (S/.)")]
        public float TotalDebt { get; set; }
        [Display(Name = "Monto por Pagar (S/.)")]
        public float AmountToPay { get; set; }
        [Display(Name = "Dinero Devuelto Parcial (S/.)")]
        public float PartialRefund { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Fecha Préstamo")]
        public DateTime LoanDate { get; set; }

        [Display(Name = "Fecha Pactada Devolución")]
        public DateTime AgreedRepaymentDate { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Notas")]
        public string? Notes { get; set; }
    }
}
