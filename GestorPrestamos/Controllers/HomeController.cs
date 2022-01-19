using GestorPrestamos.Domain.Entities;
using GestorPrestamos.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;
using GestorPrestamos.Domain.Interfaces;
using GestorPrestamos.Domain.Interfaces.Repository;
using GestorPrestamos.Domain.MasterData;

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
            //var x = _loanReceivableService.GetAllLoanReceivable();
            var x = deudoresDictionary.GetDeudoresByAlias();
            return Json(x);
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