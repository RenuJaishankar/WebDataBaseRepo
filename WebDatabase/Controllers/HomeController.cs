using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebDatabase.Models;

namespace WebDatabase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Models.ICRUD _crud;

        public HomeController(ILogger<HomeController> logger, Models.ICRUD crud)
        {
            _crud = crud;
            _logger = logger;
        }

        public IActionResult Index()
        {
            Models.Track t = _crud.FindById(1);
            return View(t);
        }

        public IActionResult Find(int id)
        {
            Models.Track t = _crud.FindById(id);
            return View("Index",t);
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
