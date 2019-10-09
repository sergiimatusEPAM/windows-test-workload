using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DemoApp.Models;

namespace DemoApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()

        {
            ViewData["host"] = HttpContext.Request.Host.Value.ToString();
            ViewData["Framework"] = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
            ViewData["ARCH"] = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture;
            ViewData["OS"] = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Should be Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Use to be Your contact page.";

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
