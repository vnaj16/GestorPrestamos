using System.ComponentModel.DataAnnotations;

namespace GestorPrestamos.ViewModels
{
    public class PrestamoFormViewModel
    {
        public string? Id { get; set; }
        [Display(Name = "Monto Prestado (S/.)")]
        public float BorrowedAmount { get; set; }
        [Display(Name = "Deudor")]
        public int DebtorId { get; set; }
        public List<SelectableDebtor>? SelectablesDebtors { get; set; }
        [Display(Name = "Comisión (S/.)")]
        public float Commission { get; set; }
        [Display(Name = "Interés (S/.)")]
        public float Interest { get; set; }
        [Display(Name = "Categoria")]
        public Category Category { get; set; }
        [Display(Name = "Fecha Préstamo")]
        public DateTime LoanDate { get; set; }
        [Display(Name = "Fecha Pactada Devolución")]
        public DateTime AgreedRepaymentDate { get; set; }
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        [Display(Name = "Notas")]
        public string? Notes { get; set; }
    }

    public class SelectableDebtor
    {
        public string Label { get; set; }
        public int DebtorId { get; set; }
    }

    public enum Category
    {
        [Display(Name = "Pago Servicio")]
        PagoServicio,
        [Display(Name = "Prestamo Monetario")]
        PrestamoMonetario
    }
}
