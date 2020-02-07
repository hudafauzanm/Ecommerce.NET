using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Razor.Data;
using System.Linq;

namespace Razor.Controllers
{
    public class ProdukController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public AppDbContext AppDbContext {get;set;}

        public ProdukController(ILogger<HomeController> logger,AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
            _logger = logger;
        }

        public IActionResult Detail(int id)
        {
            var item = from l in AppDbContext.Item where l.id == id select l;
            ViewBag.Detail = item;
            return View();        
        }
        public IActionResult Index(int id)
        {
            var list = from l in AppDbContext.Item select l;
            ViewBag.List = list;
            return View("Produk");        
        }
    }
}