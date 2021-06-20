using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using LightningPay.Samples.WebAppMvc.Models;

namespace LightningPay.Samples.WebAppMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILightningClient lightningClient;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILightningClient lightningClient,
            ILogger<HomeController> logger)
        {
            this.lightningClient = lightningClient;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
