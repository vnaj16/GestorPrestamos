using GestorPrestamos.Domain.Entities;
using GestorPrestamos.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;
using GestorPrestamos.Domain.Interfaces;
using GestorPrestamos.Domain.Interfaces.Repository;
using GestorPrestamos.Domain.MasterData;
using GestorPrestamos.ViewModels;

namespace GestorPrestamos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILoanReceivableService _loanReceivableService;
        private readonly IDeudoresDictionary deudoresDictionary;
        public HomeController(ILogger<HomeController> logger, ILoanReceivableService loanReceivableService, IDeudoresDictionary deudoresDictionary)
        {
            _logger = logger;
            _loanReceivableService = loanReceivableService;
            this.deudoresDictionary = deudoresDictionary;
        }

        public IActionResult Index()
        {
            Log.Information("Enter to Index Page");

            //var d1 = deudoresDictionary.DeudoresById;
            //var d2 = deudoresDictionary.DeudoresByAlias;
            return View();
        }

        public IActionResult Privacy()
        {
            //Log.Warning("Enter to Privacy Page");
            //var x = prestamoRepository.GetAll();
            var x = _loanReceivableService.GetAllLoanReceivable();
            //var x = deudoresDictionary.GetDeudoresByAlias();
            return Json(x);
        }

        public IActionResult LoansToCollectIndex()
        {
            var listLoan = _loanReceivableService.GetAllLoanReceivableWithStatusToPay();
            var statsLoansToCollect = _loanReceivableService.GetStats();
            LoansToCollectHomeViewModel result = new LoansToCollectHomeViewModel()
            {
                NumberOfCollectedLoans = statsLoansToCollect.NumberOfCollectedLoans,
                NumberOfLoansToCollect = statsLoansToCollect.NumberOfLoansToCollect,
                TotalAmountToBeCollected = statsLoansToCollect.TotalAmountToBeCollected,
                LoansToCollect = new List<LoanToCollectViewModel>()
            };

            foreach (var loan in listLoan)
            {
                result.LoansToCollect.Add(new LoanToCollectViewModel()
                {
                    Id = loan.Id,
                    Debtor = loan.IdDeudor.ToString(),
                    AmountToPay = loan.MontoPorPagar,
                    BorrowedAmount = loan.MontoPrestado,
                    Commission = loan.Comision,
                    Description = loan.Descripcion,
                    Interest = loan.Intereses,
                    LoanDate = loan.FechaPrestamo,
                    PartialRefund = loan.DineroDevueltoParcial,
                    TotalDebt = loan.DeudaTotal
                });
            }
            return View(result);
        }



        //public IActionResult GetById(string id)
        //{
        //    return Json(prestamoRepository.GetById(id));
        //}

        //public IActionResult GetByIdWithDeudorIncluded(string id)
        //{
        //    return Json(prestamoRepository.GetByIdWithDeudorEntityIncluded(id));
        //}

        //[HttpPost]
        //public IActionResult AddPrestamo([FromBody]Prestamo entity)
        //{
        //    return Json(prestamoRepository.Add(entity));
        //}

        //[HttpPut]
        //public IActionResult UpdatePrestamo([FromBody] Prestamo entity)
        //{
        //    return Json(prestamoRepository.Update(entity));
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}