using System;
using System.Threading.Tasks;
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

        [Route("pay")]
        [HttpPost]
        public async Task<IActionResult> Pay(CreateInvoiceRequest request)
        {
            if(ModelState.IsValid)
            {
                var invoice = await this.lightningClient.CreateInvoice(request.Amount, request.Description);

                return View("Invoice", new InvoiceModel()
                {
                    Id = invoice.Id,
                    Description = invoice.Memo,
                    PayementRequest = invoice.BOLT11,
                    Uri = invoice.Uri
                });
            }

            return View(nameof(Index));

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
