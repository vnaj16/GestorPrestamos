using GestorPrestamos.Data.Interfaces;
using GestorPrestamos.Models;
using GestorPrestamos.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;

namespace GestorPrestamos.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMyService myService;
        private readonly IPrestamoRepository prestamoRepository;

        public HomeController(ILogger<HomeController> logger, IMyService myService, IPrestamoRepository prestamoRepository)
        {
            _logger = logger;
            this.myService = myService;
            this.prestamoRepository = prestamoRepository;
        }

        public IActionResult Index()
        {
            Log.Information("Enter to Index Page");
            Log.Information($"Result from Service {myService.GetData()}");
            return View();
        }

        public IActionResult Privacy()
        {
            //Log.Warning("Enter to Privacy Page");
            var x = prestamoRepository.GetAll();
            return Json(x);
        }

        public IActionResult GetById(string id)
        {
            return Json(prestamoRepository.GetById(id));
        }

        public IActionResult GetByIdWithDeudorIncluded(string id)
        {
            return Json(prestamoRepository.GetByIdWithDeudorEntityIncluded(id));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}