using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Razor;
using Razor.Data;

namespace Razor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public AppDbContext AppDbContext {get;set;}

        public HomeController(ILogger<HomeController> logger,AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var item = from l in AppDbContext.Item where l.rating >= 4 select l;
            ViewBag.Item = item;
            return View();        
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        // public IActionResult Error()
        // {
        //     //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        // }
    }
}
