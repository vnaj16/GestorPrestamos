namespace GestorPrestamos.ViewModels
{
    public class LoansToCollectHomeViewModel
    {
        //For the Cards
        public float TotalAmountToBeCollected { get; set; }
        public int NumberOfCollectedLoans { get; set; }
        public int NumberOfLoansToCollect { get; set; }
        //For the Table
        public List<LoanToCollectViewModel> LoansToCollect { get; set; }
    }
}
