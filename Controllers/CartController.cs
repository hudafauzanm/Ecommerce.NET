using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Razor.Data;
using Razor.Models;
using System.Linq;

namespace Razor.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public AppDbContext AppDbContext {get;set;}

        public CartController(ILogger<HomeController> logger,AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var item = from l in AppDbContext.Transaksi from y in AppDbContext.Item where l.Item_id == y.id select y;
            ViewBag.Cart = item; 
            return View("Cart");        
        }

        public IActionResult Add(int id)
        {
            var item = from l in AppDbContext.Transaksi from y in AppDbContext.Item where l.Item_id == y.id select y;
            ViewBag.Cart = item; 
            Cart cart = null;
            if(!AppDbContext.Cart.Any())
            {
                cart = new Cart();

                Transaksi transaksibaru = new Transaksi()
                {
                    Cart = cart,
                    Item_id = id
                };
                AppDbContext.Transaksi.Add(transaksibaru);
            }
            var x = from l in AppDbContext.Transaksi select l;
            foreach(var trans in x)
            {
                if(trans.Cart_id == 3 && trans.Item_id == id)
                {
                    return RedirectToAction("Index","Produk");
                }
            }
            var cartid = AppDbContext.Cart.Find(3);
            Transaksi transaksilama = new Transaksi()
            {
                Cart_id = cartid.id, 
                Item_id = id,
            };
            AppDbContext.Transaksi.Add(transaksilama);
            AppDbContext.SaveChanges();
            
            return View("Cart");
        }

        public IActionResult Delete(int id)
        {
            var cart = AppDbContext.Transaksi.Find(id,3);
            AppDbContext.Transaksi.Remove(cart);
            AppDbContext.SaveChanges();
            return RedirectToAction("Index","Cart");
        }

    }
}