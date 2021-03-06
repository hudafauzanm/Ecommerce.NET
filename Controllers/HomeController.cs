﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Razor;
using Razor.Data;
using Razor.Models;
using Microsoft.Extensions.Configuration;

namespace Razor.Controllers
{
    public class HomeController : Controller
    {
         public IConfiguration Configuration;
        private readonly ILogger<HomeController> _logger;
        public AppDbContext AppDbContext {get;set;}

        public HomeController(ILogger<HomeController> logger,AppDbContext appDbContext,IConfiguration configuration)
        {
            AppDbContext = appDbContext;
            _logger = logger;
            Configuration = configuration;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index(int Sort,int? page,int PerPage,string Search = "")
        {
            var user_id = HttpContext.Session.GetString("Id");
            var cart = (from t in AppDbContext.Transaksi where t.User_id == int.Parse(user_id) select t.Cart_id).Distinct();
            var receive_chat = from l in AppDbContext.Chat where l.receiver_id == int.Parse(user_id) || l.sender_id == int.Parse(user_id) orderby l.created_at select l;
            var unreadchat = (from l in AppDbContext.Chat where l.read_at == DateTime.Parse("0001-01-01 00:00:00.0000000") && l.receiver_id == int.Parse(user_id) orderby l.created_at select l).Count();
            var sender_chat = from l in AppDbContext.Chat where l.sender_id == int.Parse(user_id) orderby l.created_at select l;
            //var user_id = HttpContext.Session.GetString("Id");
            ViewBag.UnRead = unreadchat;
            ViewBag.RChat = receive_chat;
            ViewBag.SChat = sender_chat;
            ViewBag.User = user_id;
            ViewBag.CartId = cart;
            ViewBag.Sort = Sort;
            ViewBag.Search = Search;
            ViewBag.PerPage = PerPage;

            var user_list = from l in AppDbContext.User where l.role == 1 select l;
            ViewBag.UL = user_list;

            if(!String.IsNullOrEmpty(Search) || !String.IsNullOrWhiteSpace(Search))
            {
               var item = from l in AppDbContext.Item where l.item_name.Contains(Search) || l.description.Contains(Search) where l.rating >= 4 select l;
               var pager = new Pager(item.Count(), page);
               var viewModel = new IndexViewModel
                {
                    Item = item.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                return View(viewModel);
            }
            if(Sort != 0)
            {
                var x = Sorting(Sort,page);
                return View(x);
            }
            if(PerPage != 0)
            {
                var item = from l in AppDbContext.Item where l.rating >= 4 select l;
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
               var item = from l in AppDbContext.Item where l.rating >= 4 select l;
               var pager = new Pager(item.Count(), page);
               var viewModel = new IndexViewModel
                {
                    Item = item.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                return View(viewModel);
            }       
        }

        public IActionResult Privacy()
        {
            Console.WriteLine(Configuration["mypassword"]);
            return View();
        }

        [HttpGet]
        public IActionResult UpdateReadChat()
        {
            Console.WriteLine("Berhasil Berhasil");
            var user_id = HttpContext.Session.GetString("Id");
            var receiver = from r in AppDbContext.Chat where r.receiver_id == int.Parse(user_id) select r;
            foreach(var recv in receiver)
            {
                var find = AppDbContext.Chat.Find(recv.id);
                find.read_at = DateTime.Now;
            }
            AppDbContext.SaveChanges();
            var unreadchat = (from l in AppDbContext.Chat where l.read_at == DateTime.Parse("0001-01-01 00:00:00.0000000") && l.receiver_id == int.Parse(user_id) orderby l.created_at select l).Count();
            return Ok(unreadchat);
        }

        public IActionResult UpdateNotifChat()
        {
            var notif = from r in AppDbContext.Notification select r;
            foreach(var recv in notif)
            {
                var find = AppDbContext.Notification.Find(recv.id);
                find.read_at = DateTime.Now;
            }
            AppDbContext.SaveChanges();
            var unreadchat = (from l in AppDbContext.Notification where l.read_at == DateTime.Parse("0001-01-01 00:00:00.0000000") orderby l.created_at select l).Count();
            return Ok(unreadchat);
        }
        [Authorize]
        public IActionResult Admin()
        {
            
            var user_list = from l in AppDbContext.User where l.role == 2 select l;
            ViewBag.UL = user_list;
            var user_id = HttpContext.Session.GetString("Id");
            ViewBag.User = user_id;
            var token = HttpContext.Session.GetString("JWTToken");
            var jwtSec  = new JwtSecurityTokenHandler();
            var securityToken = jwtSec.ReadToken(token) as JwtSecurityToken;
            var sub = securityToken?.Claims.First(Claim => Claim.Type == "sub").Value;
            var item = from l in AppDbContext.Item select l;
            ViewBag.Item = item;
            ViewBag.Username = sub;
            var receive_chat = from l in AppDbContext.Chat where l.receiver_id == int.Parse(user_id) || l.sender_id == int.Parse(user_id) orderby l.created_at select l;
            var unreadchat = (from l in AppDbContext.Chat where l.read_at == DateTime.Parse("0001-01-01 00:00:00.0000000") && l.receiver_id == int.Parse(user_id) orderby l.created_at select l).Count();
            var sender_chat = from l in AppDbContext.Chat where l.sender_id == int.Parse(user_id) orderby l.created_at select l;
            //var user_id = HttpContext.Session.GetString("Id");
            ViewBag.UnRead = unreadchat;
            ViewBag.RChat = receive_chat;
            ViewBag.SChat = sender_chat;
            return View();
        }

        public IActionResult Add(string Name_item,double Price,int Rating,string Img_url,string Deskripsi)
        {
          Item item = new Item()
          {
              item_name = Name_item,
              price = Price,
              rating = Rating,
              url_img = Img_url,
              description = Deskripsi
          };
          AppDbContext.Item.Add(item);
          AppDbContext.SaveChanges();
          Console.WriteLine("Berhasil Tambah Data");

          return RedirectToAction("Admin","Home");
        }

        public IActionResult Edit(int Id_item,string Name_item,double Price,int Rating,string Img_url,string Deskripsi)
        {
          var x = AppDbContext.Item.Find(Id_item);
          x.item_name = Name_item;
          x.price = Price;
          x.rating = Rating;
          x.url_img = Img_url;
          x.description = Deskripsi;
          AppDbContext.SaveChanges();
          Console.WriteLine("Berhasil Edit Data");

          return RedirectToAction("Admin","Home");
        }

        public IActionResult Delete(int Id_item)
        {
          var x = AppDbContext.Item.Find(Id_item);
          AppDbContext.Remove(x);
          AppDbContext.SaveChanges();
          Console.WriteLine("Berhasil Hapus Data");

          return RedirectToAction("Admin","Home");
        }

        public IndexViewModel Sorting(int Sort,int? page)
        {   
            var value = Sort;
            ViewBag.Sort = value;
    
            if(Sort == 1)
            {
                var data = AppDbContext.Item.OrderBy(s => s.item_name).Where(s => s.rating >= 4);
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
                var data = AppDbContext.Item.OrderByDescending(s => s.item_name).Where(s => s.rating >= 4);
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
                var data = AppDbContext.Item.OrderBy(s => s.price).Where(s => s.rating >= 4);
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
                var data = AppDbContext.Item.OrderByDescending(s => s.price).Where(s => s.rating >= 4);
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
                var data = AppDbContext.Item.OrderBy(s => s.created_at).Where(s => s.rating >= 4);
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
                var data = AppDbContext.Item.OrderByDescending(s => s.created_at).Where(s => s.rating >= 4);
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
                var data = AppDbContext.Item.OrderBy(s => s.published_at).Where(s => s.rating >= 4);
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
                var data = AppDbContext.Item.OrderByDescending(s => s.published_at).Where(s => s.rating >= 4);
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
                var data = AppDbContext.Item.Where(s => s.rating >= 4);
                var pager = new Pager(data.Count(), page);
                var viewModel = new IndexViewModel
                {
                    Item = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                    Pager = pager
                };
                return viewModel;
            }
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return RedirectToAction("Index","User");
        }
    }
}
