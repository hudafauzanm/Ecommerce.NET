using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Razor.Data;
using Razor.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize]
        public IActionResult Index(int id)
        {
            var user_id = HttpContext.Session.GetString("Id");
            var item = from l in AppDbContext.Transaksi where l.User_id == int.Parse(user_id) select l.Item; 
            var cart = (from t in AppDbContext.Transaksi where t.User_id == int.Parse(user_id) where t.Cart_id == id select t.Cart_id).Distinct();
            ViewBag.Id = id;
            ViewBag.CartId = cart;
            ViewBag.User = user_id;
            ViewBag.Cart = item; 
            return View("Cart");        
        }

        public IActionResult Add(string Id,string Cart)
        {
            var item = from l in AppDbContext.Transaksi from y in AppDbContext.Item where l.Item_id == y.id select y;
            ViewBag.Cart = item; 
            var id_user = HttpContext.Session.GetString("Id");
            Cart cart = null;
            if(Cart == null)
            {
                Console.WriteLine("==============================Cart Baru");
                Console.WriteLine("Masuk Cart baru");
                cart = new Cart();

                Transaksi transaksibaru = new Transaksi()
                {
                    User_id = int.Parse(id_user),
                    Cart = cart,
                    Item_id = int.Parse(Id)
                };
                AppDbContext.Transaksi.Add(transaksibaru);
            }
            else
            {
                Console.WriteLine("==============================Cart Lama");
                Console.WriteLine("Masuk Cart Lama");
                var cartid = AppDbContext.Cart.Find(int.Parse(Cart));
                Transaksi transaksilama = new Transaksi()
                {
                    User_id = int.Parse(id_user),
                    Cart_id = cartid.id, 
                    Item_id = int.Parse(Id)
                };
                AppDbContext.Transaksi.Add(transaksilama);
            }
            var x = from l in AppDbContext.Transaksi select l;
            foreach(var trans in x)
            {
                if(trans.User_id == int.Parse(id_user) && trans.Item_id == int.Parse(Id))
                {
                    return RedirectToAction("Index","Produk");
                }
            }
            AppDbContext.SaveChanges();
            return RedirectToAction("Index","Produk");
        }

        public IActionResult Delete(int id,int cartid)
        {
            var user_id = HttpContext.Session.GetString("Id");
            Console.WriteLine(id);
            Console.WriteLine(cartid);
            var cart = AppDbContext.Transaksi.Find(id,cartid,int.Parse(user_id));
            Console.WriteLine("===================================");
            Console.WriteLine(cart);
            AppDbContext.Transaksi.Remove(cart);
            AppDbContext.SaveChanges();
            return RedirectToAction("Index","Cart");
        }

        public IActionResult Payment(int total_price,int cart_id)
        {
            var cart = AppDbContext.Cart.Find(cart_id);
            cart.total = total_price;
            AppDbContext.SaveChanges();
            return RedirectToAction("Index","Purchase");
        }
    }
}