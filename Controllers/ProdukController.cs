using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Razor.Data;
using Razor.Models;
using System;
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

        public IActionResult AddCart()
        {
            var user_id = HttpContext.Session.GetString("Id");
            Cart cart = new Cart();
            Transaksi transaksi = new Transaksi()
            {
                User_id = int.Parse(user_id),
                Cart = cart,
            };
            AppDbContext.Transaksi.Add(transaksi);
            AppDbContext.Cart.Add(cart);
            AppDbContext.SaveChanges();
            return PartialView();
        }
        [Authorize]
        public IActionResult Detail(int id)
        {
            var user_id = HttpContext.Session.GetString("Id");
            var item = from l in AppDbContext.Item where l.id == id select l;
            var cart = (from c in AppDbContext.Transaksi where c.User_id == int.Parse(user_id) select c.Cart).Distinct();
            var cartid = (from c in AppDbContext.Transaksi where c.User_id == int.Parse(user_id) select c.Cart.id).Distinct();
            //var user_id = HttpContext.Session.GetString("Id");
            ViewBag.User = user_id;
            ViewBag.Id = id;
            ViewBag.CartId = cartid;
            ViewBag.Cart = cart;
            ViewBag.Detail = item;

            var user_list = from l in AppDbContext.User where l.role == 1 select l;
            ViewBag.UL = user_list;
            var receive_chat = from l in AppDbContext.Chat where l.receiver_id == int.Parse(user_id) || l.sender_id == int.Parse(user_id) orderby l.created_at select l;
            var unreadchat = (from l in AppDbContext.Chat where l.read_at == DateTime.Parse("0001-01-01 00:00:00.0000000") && l.receiver_id == int.Parse(user_id) orderby l.created_at select l).Count();
            var sender_chat = from l in AppDbContext.Chat where l.sender_id == int.Parse(user_id) orderby l.created_at select l;
            //var user_id = HttpContext.Session.GetString("Id");
            ViewBag.UnRead = unreadchat;
            ViewBag.RChat = receive_chat;
            ViewBag.SChat = sender_chat;
            return View();        
        }
        [Authorize]
        public IActionResult Index(int Sort,int? page,int PerPage,string Search = "")
        {
            var user_id = HttpContext.Session.GetString("Id");
            var user_list = from l in AppDbContext.User where l.role == 1 select l;
            ViewBag.UL = user_list;
            var cart = (from t in AppDbContext.Transaksi where t.User_id == int.Parse(user_id) select t.Cart_id).Distinct();
            //var user_id = HttpContext.Session.GetString("Id");
            ViewBag.User = user_id;
            ViewBag.CartId = cart;
            ViewBag.Sort = Sort;
            ViewBag.Search = Search;
            var receive_chat = from l in AppDbContext.Chat where l.receiver_id == int.Parse(user_id) || l.sender_id == int.Parse(user_id) orderby l.created_at select l;
            var unreadchat = (from l in AppDbContext.Chat where l.read_at == DateTime.Parse("0001-01-01 00:00:00.0000000") && l.receiver_id == int.Parse(user_id) orderby l.created_at select l).Count();
            var sender_chat = from l in AppDbContext.Chat where l.sender_id == int.Parse(user_id) orderby l.created_at select l;
            //var user_id = HttpContext.Session.GetString("Id");
            ViewBag.UnRead = unreadchat;
            ViewBag.RChat = receive_chat;
            ViewBag.SChat = sender_chat;

            if(!String.IsNullOrEmpty(Search) || !String.IsNullOrWhiteSpace(Search))
            {
               var item = from l in AppDbContext.Item where l.item_name.Contains(Search) || l.description.Contains(Search) where l.rating >= 4 select l;
               var pager = new Pager(item.Count(), page);
               var viewModel = new IndexViewModel
                {
                    Item = item.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                return View("Produk",viewModel);
            }
            if(Sort != 0)
            {
                var x = Sorting(Sort,page);
                return View(x);
            }
            if(PerPage != 0)
            {
                var item = from l in AppDbContext.Item select l;
                var pager = new Pager(item.Count(),page,PerPage);
                var viewModel = new IndexViewModel
                {
                    Item = item.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                return View(viewModel);
            }
            else
            {
               var item = from l in AppDbContext.Item select l;
               var pager = new Pager(item.Count(), page);
               var viewModel = new IndexViewModel
                {
                    Item = item.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                return View("Produk",viewModel);
            }       
        }

        public IndexViewModel Sorting(int Sort,int? page)
        {   
            var value = Sort;
            ViewBag.Sort = value;
    
            if(Sort == 1)
            {
                var data = AppDbContext.Item.OrderBy(s => s.item_name);
                var pager = new Pager(data.Count(), page);
                var viewModel = new IndexViewModel
                {
                    Item = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                return (viewModel);
            }
            if(Sort == 2)
            {
                var data = AppDbContext.Item.OrderByDescending(s => s.item_name);
                var pager = new Pager(data.Count(), page);
                var viewModel = new IndexViewModel
                {
                    Item = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                return viewModel;  
            }
            if(Sort == 3)
            {
                var data = AppDbContext.Item.OrderBy(s => s.price);
                var pager = new Pager(data.Count(), page);
                var viewModel = new IndexViewModel
                {
                    Item = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                return viewModel;
            }
            if(Sort == 4)
            {
                var data = AppDbContext.Item.OrderByDescending(s => s.price);
                var pager = new Pager(data.Count(), page);
                var viewModel = new IndexViewModel
                {
                    Item = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                return viewModel;
            }
            if(Sort == 5)
            {
                var data = AppDbContext.Item.OrderBy(s => s.created_at);
                var pager = new Pager(data.Count(), page);
                var viewModel = new IndexViewModel
                {
                    Item = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                return viewModel;
            }
            if(Sort == 6)
            {
                var data = AppDbContext.Item.OrderByDescending(s => s.created_at);
                var pager = new Pager(data.Count(), page);
                var viewModel = new IndexViewModel
                {
                    Item = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                return viewModel;
            }
            if(Sort == 7)
            {
                var data = AppDbContext.Item.OrderBy(s => s.published_at);
                var pager = new Pager(data.Count(), page);
                var viewModel = new IndexViewModel
                {
                    Item = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                return viewModel;
            }
            if(Sort == 8)
            {
                var data = AppDbContext.Item.OrderByDescending(s => s.published_at);
                var pager = new Pager(data.Count(), page);
                var viewModel = new IndexViewModel
                {
                    Item = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                return viewModel;
            }
            else
            {
                var data = AppDbContext.Item;
                var pager = new Pager(data.Count(), page);
                var viewModel = new IndexViewModel
                {
                    Item = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                return viewModel;
            }
        }
    }
}