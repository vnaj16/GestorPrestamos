using GestorPrestamos.Domain.Entities;
using GestorPrestamos.Domain.Interfaces;
using GestorPrestamos.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestorPrestamos.Controllers
{
    public class LoanReceivableController : Controller
    {
        private readonly ILoanReceivableService _loanReceivableService;
        private readonly IMasterDataService _masterDataService;

        public LoanReceivableController(ILoanReceivableService loanReceivableService, IMasterDataService masterDataService)
        {
            _loanReceivableService = loanReceivableService;
            _masterDataService = masterDataService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RegisterLoan()
        {
            PrestamoFormViewModel prestamoForm = new PrestamoFormViewModel()
            {
                SelectablesDebtors = new List<SelectableDebtor>()
            };

            foreach (var item in _masterDataService.GetDebtors())
            {
                prestamoForm.SelectablesDebtors.Add(new SelectableDebtor()
                {
                    DebtorId = item.Key,
                    Label = item.Value.Alias
                });
            }

            return View(prestamoForm);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterLoan(PrestamoFormViewModel prestamoForm)
        {            //TODO: Aca armar Descripción
            //Categoria-Descripción-Mes
            if (ModelState.IsValid)
            {
                Prestamo prestamo = new Prestamo()
                {
                    Comision = prestamoForm.Commission,
                    Descripcion = $"{prestamoForm.Category} {prestamoForm.Description} {GetMonth()}",
                    FechaPactadaDevolucion = prestamoForm.AgreedRepaymentDate,
                    FechaPrestamo = prestamoForm.LoanDate,
                    IdDeudor = prestamoForm.DebtorId,
                    Intereses = prestamoForm.Interest,
                    MontoPrestado = prestamoForm.BorrowedAmount,
                    Notas = prestamoForm.Notes
                };
                var response = _loanReceivableService.RegisterLoanReceivable(prestamo);
                prestamoForm.Id = response.RegisteredEntity.Id;
            
                prestamoForm.SelectablesDebtors = new List<SelectableDebtor>();
                foreach (var item in _masterDataService.GetDebtors())
                {
                    prestamoForm.SelectablesDebtors.Add(new SelectableDebtor()
                    {
                        DebtorId = item.Key,
                        Label = item.Value.Alias
                    });
                }
                return Json(response.RegisteredEntity);
                //return View(prestamoForm);
            }
            else
            {
                return View(prestamoForm);
            }

        }

        private string GetMonth()
        {
            var month = DateTime.Now.Month;
            switch (month)
            {
                case 1:
                    return "Enero";
                case 2:
                    return "Febrero";
                case 3:
                    return "Marzo";
                case 4:
                    return "Abril";
                case 5:
                    return "Mayo";
                case 6:
                    return "Junio";
                case 7:
                    return "Julio";
                case 8:
                    return "Agosto";
                case 9:
                    return "Septiembre";
                case 10:
                    return "Octubre";
                case 11:
                    return "Noviembre";
                case 12:
                    return "Diciembre";
                default:
                    return string.Empty;
            }
        }
    }
}
