using Microsoft.AspNetCore.Mvc;
using Razor.Data;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using Razor.Models;
// using System.Text.Json;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Razor.Controllers
{
    
    public class PurchaseController : Controller
    {
        public AppDbContext AppDbContext {get;set;}
        public PurchaseController(AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            var user_id = HttpContext.Session.GetString("Id");
            var item = from t in AppDbContext.Transaksi where t.User_id == int.Parse(user_id) select t.Item;
            var cart_total = from t in AppDbContext.Transaksi where t.User_id == int.Parse(user_id) select t.Cart.total;
            var cart_id = from t in AppDbContext.Transaksi where t.User_id == int.Parse(user_id) select t.Cart.id;
            var x = cart_total.ToList();
            var y = cart_id.ToList();
            var total = x.First();
            var id = y.First();
            var cart = (from c in AppDbContext.Transaksi where c.User_id == int.Parse(user_id) select c.Cart).Distinct();
            ViewBag.CartId = cart;
            ViewBag.Item = item;
            ViewBag.User = user_id;
            ViewBag.Cart = total;
            ViewBag.CartID = id;
            
            return View("Purchase");
      
        }
        public async Task<IActionResult> Payment(string firstName,string lastName,int username,string email,string address,string zip,string paymentMethod,string Order_id,string total_cart,string bank_type)
        {
            var user_id = HttpContext.Session.GetString("Id");
            var client = new HttpClient();
            var transaksi = FormatPay(paymentMethod,Order_id,total_cart,bank_type);
            var hasil = new StringContent(transaksi,Encoding.UTF8,"application/json");
            var token = Encoding.ASCII.GetBytes("SB-Mid-server-3xaB-yb0Px1mIPnaPLeNFXIF:");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(token));
            var response = await client.PostAsync("https://api.sandbox.midtrans.com/v2/charge",hasil);
            var json = response.Content.ReadAsStringAsync().Result;
            var data_purchase = JsonConvert.DeserializeObject<Transaction_Details>(json);
            AppDbContext.Add(data_purchase);
            AppDbContext.SaveChanges();
            Purchase purchase = new Purchase()
            {
                nama_pemesan = firstName +" "+ lastName,
                alamat = address,
                email = email,
                kodepos = zip,
                User_id = username,
                transaction_id = data_purchase.id,
                created_at = DateTime.Now,
                published_at = DateTime.Now
                
            };
            AppDbContext.Add(purchase);
            AppDbContext.SaveChanges();
            var pembayaran = (from pur in AppDbContext.Purchase where pur.User_id == int.Parse(user_id) orderby pur.transaction_id descending select pur.Transaction_Details).FirstOrDefault(); 
            var va = (from v in AppDbContext.Purchase where v.User_id == int.Parse(user_id) orderby v.transaction_id descending select v.Transaction_Details.va_numbers).FirstOrDefault();
            ViewBag.Va = va;
            ViewBag.Detail = pembayaran;
            var cartid = (from c in AppDbContext.Transaksi where c.User_id == int.Parse(user_id) select c.Cart.id).Distinct();
            ViewBag.CartId = cartid;
            return View("Receipt");
        }

        public string FormatPay(string paymentMethod,string Order_id,string total_cart,string bank_type)
        {
            var user_id = HttpContext.Session.GetString("Id");
            var data = "";
            if(paymentMethod == "gopay")
            {
                var transaksi = new
                {
                    payment_type = paymentMethod,
                    transaction_details = new 
                    {
                        order_id = "Order"+"-"+Order_id+"G"+user_id,
                        gross_amount = total_cart
                    },
                    gopay = new 
                    {
                        enable_callback = true,
                        callback_url = "someapps://callback"
                    }
                };
                data = JsonConvert.SerializeObject(transaksi);
                return data;
            }
            if(paymentMethod == "bank_transfer")
            {
                if(bank_type == "bca")
                {
                    var transaksi = new 
                    {
                        payment_type = paymentMethod,
                        transaction_details = new 
                        {
                            order_id = "Order"+"-"+Order_id+"BCA1"+user_id,
                            gross_amount = total_cart
                        },
                        bank_transfer = new 
                        {
                            bank = bank_type
                        }
                    };
                    data = JsonConvert.SerializeObject(transaksi);
                    return data;
                }
                if(bank_type == "echannel")
                {
                    var transaksi = new 
                    {
                        payment_type = bank_type,
                        transaction_details = new 
                        {
                            order_id = "Order"+"-"+Order_id+"MAN"+user_id,
                            gross_amount = total_cart
                        },
                    };
                    data = JsonConvert.SerializeObject(transaksi);
                    return data;
                }
                if(bank_type == "bni")
                {
                    var transaksi = new 
                    {
                        payment_type = paymentMethod,
                        transaction_details = new 
                        {
                            order_id = "Order"+"-"+Order_id+"BNI1"+user_id,
                            gross_amount = total_cart
                        },
                        bank_transfer = new 
                        {
                            bank = bank_type
                        }
                    };
                    data = JsonConvert.SerializeObject(transaksi);
                    return data;
                }
                if(bank_type == "permata")
                {
                    Console.WriteLine(bank_type);
                    var transaksi = new 
                    {
                        payment_type = bank_type,
                        transaction_details = new 
                        {
                            order_id = "Order"+"-"+Order_id+"Per"+user_id,
                            gross_amount = int.Parse(total_cart)
                        },
                    };
                    data = JsonConvert.SerializeObject(transaksi);
                    Console.WriteLine(data);
                    return data;
                }
                
            }
            if(paymentMethod == "direct_debit")
            {
                if(bank_type == "bca_klikpay")
                {
                    var transaksi = new 
                    {
                        payment_type = bank_type,
                        transaction_details = new 
                        {
                            order_id = "Order"+"-"+Order_id+user_id,
                            gross_amount = total_cart
                        },
                        bca_klikpay = new 
                        {
                            description = "Pembelian"
                        }
                    };
                    data = JsonConvert.SerializeObject(transaksi);
                    return data;
                }
                if(bank_type == "cimb_clicks")
                {
                    var transaksi = new 
                    {
                        payment_type = bank_type,
                        transaction_details = new 
                        {
                            order_id = "Order"+"-"+Order_id+user_id,
                            gross_amount = total_cart
                        },
                        cimb_clicks = new 
                        {
                            description = "Pembelian"
                        }
                    };
                    data = JsonConvert.SerializeObject(transaksi);
                    return data;
                }
                if(bank_type == "danamon_online")
                {
                    var transaksi = new 
                    {
                        payment_type = bank_type,
                        transaction_details = new 
                        {
                            order_id = "Order"+"-"+Order_id+user_id,
                            gross_amount = total_cart
                        },
                    };
                    data = JsonConvert.SerializeObject(transaksi);
                    return data;
                }
                if(bank_type == "bri_epay")
                {
                    var transaksi = new 
                    {
                        payment_type = bank_type,
                        transaction_details = new 
                        {
                            order_id = "Order"+"-"+Order_id+user_id,
                            gross_amount = total_cart
                        },
                    };
                    data = JsonConvert.SerializeObject(transaksi);
                    return data;
                }
                if(bank_type == "akulaku")
                {
                    var transaksi = new 
                    {
                        payment_type = bank_type,
                        transaction_details = new 
                        {
                            order_id = "Order"+"-"+Order_id+user_id,
                            gross_amount = total_cart
                        },
                    };
                    data = JsonConvert.SerializeObject(transaksi);
                    return data;
                }
            }
          return data;
        }

        public IActionResult Transaksi()
        {
            var user_id = HttpContext.Session.GetString("Id");
            var pembayaran = from pur in AppDbContext.Purchase where pur.User_id == int.Parse(user_id) orderby pur.transaction_id select pur.Transaction_Details;
            var cart = (from c in AppDbContext.Transaksi where c.User_id == int.Parse(user_id) select c.Cart).Distinct();
            ViewBag.CartId = cart;
            ViewBag.Transaksi = pembayaran;
            return View("Transaksi");
        }

        public async Task<IActionResult> Refresh()
        {
            var client = new HttpClient();
            var user_id = HttpContext.Session.GetString("Id");
            var pembayaran = (from v in AppDbContext.Purchase where v.User_id == int.Parse(user_id) orderby v.transaction_id descending select v.Transaction_Details);
           
            foreach(var order in pembayaran)
            {
                var token = Encoding.ASCII.GetBytes("SB-Mid-server-3xaB-yb0Px1mIPnaPLeNFXIF:");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(token));
                var response = await client.GetAsync($"https://api.sandbox.midtrans.com/v2/{order.order_id}/status");
                var json = response.Content.ReadAsStringAsync().Result;
                var data_purchase = JsonConvert.DeserializeObject<Transaction_Details>(json);;
                var order_id = AppDbContext.Transaction_Details.Find(order.id);
                //Console.WriteLine(data_purchase.transaction_status);
                order_id.transaction_status = data_purchase.transaction_status;
            }
            AppDbContext.SaveChanges();
            return RedirectToAction("Transaksi","Purchase");
        }
    }
}