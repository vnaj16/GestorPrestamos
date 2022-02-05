using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorPrestamos.Domain.Utils
{
    public class StatsLoanToCollect
    {
        public float TotalAmountToBeCollected { get; set; }
        public int NumberOfCollectedLoans { get; set; }
        public int NumberOfLoansToCollect { get; set; }
    }
}
