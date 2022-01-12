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

        public HomeController(ILogger<HomeController> logger, IMyService myService)
        {
            _logger = logger;
            this.myService = myService;
        }

        public IActionResult Index()
        {
            Log.Information("Enter to Index Page");
            Log.Information($"Result from Service {myService.GetData()}");
            return View();
        }

        public IActionResult Privacy()
        {
            Log.Warning("Enter to Privacy Page");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}