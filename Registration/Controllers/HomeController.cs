using Microsoft.AspNetCore.Mvc;
using Registration.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Registration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HttpContext.Session.SetString("mykey", "programer");
            return View();
        }

        public IActionResult About()
        {
            if (HttpContext.Session.GetString("mykey") != null)
            {
                ViewBag.Data = HttpContext.Session.GetString("mykey").ToString();
            }

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