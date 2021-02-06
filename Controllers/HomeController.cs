using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Accounting.Client.Models;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Accounting.Client.Controllers
{
     public class HomeController : Controller
    {
        private readonly string base_url = "http://localhost:44357/";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["TransactionType"] = "Maaş";
            Random rnd = new Random();
            ViewData["TransactionObjectId"] = rnd.Next(100, 9999);
            ViewData["Amount"] = rnd.Next(3000, 9999);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PushAsync(FisModel fisModel)
        {
            string json = JsonConvert.SerializeObject(fisModel);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            var client = new HttpClient();

            var response = await client.PostAsync(base_url+"AddQueue", data);
            string result = await response.Content.ReadAsStringAsync();
            
            //close out the client
            client.Dispose();
            
            return RedirectToAction("Index");
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
