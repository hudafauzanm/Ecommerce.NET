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
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Threading;

namespace Razor.Controllers
{
    
    public class PurchaseController : Controller
    {
        public AppDbContext AppDbContext {get;set;}
        public PurchaseController(AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }
        [Authorize]
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
            //var user_id = HttpContext.Session.GetString("Id");
            //ViewBag.User = user_id;
            ViewBag.CartId = cart;
            ViewBag.Item = item;
            ViewBag.User = user_id;
            ViewBag.Cart = total;
            ViewBag.CartID = id;
            var user_list = from l in AppDbContext.User where l.role == 1 select l;
            ViewBag.UL = user_list;
            var receive_chat = from l in AppDbContext.Chat where l.receiver_id == int.Parse(user_id) || l.sender_id == int.Parse(user_id) orderby l.created_at select l;
            var unreadchat = (from l in AppDbContext.Chat where l.read_at == DateTime.Parse("0001-01-01 00:00:00.0000000") && l.receiver_id == int.Parse(user_id) orderby l.created_at select l).Count();
            var sender_chat = from l in AppDbContext.Chat where l.sender_id == int.Parse(user_id) orderby l.created_at select l;
            //var user_id = HttpContext.Session.GetString("Id");
            ViewBag.UnRead = unreadchat;
            ViewBag.RChat = receive_chat;
            ViewBag.SChat = sender_chat;
            
            return View("Purchase");
      
        }
        public async Task<IActionResult> Payment(string firstName,string lastName,int username,string email,string address,string zip,string paymentMethod,string Order_id,string total_cart,string bank_type)
        {
            var user_id = HttpContext.Session.GetString("Id");
            var client = new HttpClient();
            var transaksi = FormatPay(firstName,lastName,email,paymentMethod,Order_id,total_cart,bank_type);
            var hasil = new StringContent(transaksi,Encoding.UTF8,"application/json");
            var token = Encoding.ASCII.GetBytes("SB-Mid-server-3xaB-yb0Px1mIPnaPLeNFXIF:");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(token));
            var response = await client.PostAsync("https://api.sandbox.midtrans.com/v2/charge",hasil);
            var json = response.Content.ReadAsStringAsync().Result;
            var data_purchase = JsonConvert.DeserializeObject<Transaction_Details>(json);
            AppDbContext.Add(data_purchase);
            AppDbContext.SaveChanges();
            var receive_chat = from l in AppDbContext.Chat where l.receiver_id == int.Parse(user_id) || l.sender_id == int.Parse(user_id) orderby l.created_at select l;
            var unreadchat = (from l in AppDbContext.Chat where l.read_at == DateTime.Parse("0001-01-01 00:00:00.0000000") && l.receiver_id == int.Parse(user_id) orderby l.created_at select l).Count();
            var sender_chat = from l in AppDbContext.Chat where l.sender_id == int.Parse(user_id) orderby l.created_at select l;
            //var user_id = HttpContext.Session.GetString("Id");
            ViewBag.UnRead = unreadchat;
            ViewBag.RChat = receive_chat;
            ViewBag.SChat = sender_chat;
            var user_list = from l in AppDbContext.User where l.role == 1 select l;
            ViewBag.UL = user_list;
            var cart = (from t in AppDbContext.Transaksi where t.User_id == int.Parse(user_id) select t.Cart_id).Distinct();
            ViewBag.CartId = cart;
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
            //var mailservice = MailService(email);
            var pembayaran = (from pur in AppDbContext.Purchase where pur.User_id == int.Parse(user_id) orderby pur.transaction_id descending select pur.Transaction_Details).FirstOrDefault(); 
            var va = (from v in AppDbContext.Purchase where v.User_id == int.Parse(user_id) orderby v.transaction_id descending select v.Transaction_Details.va_numbers).FirstOrDefault();
            ViewBag.Va = va;
            ViewBag.Detail = pembayaran;
            var cartid = (from c in AppDbContext.Transaksi where c.User_id == int.Parse(user_id) select c.Cart.id).Distinct();
            ViewBag.CartId = cartid;
            Thread task = new Thread(()=>MailService(email));
            task.Start();
            return View("Receipt");
        }

        public string FormatPay(string firstName,string lastName,string email,string paymentMethod,string Order_id,string total_cart,string bank_type)
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
                            order_id = "Order"+"-"+Order_id+"BCA"+user_id,
                            gross_amount = total_cart
                        },
                        customer_details = new
                        {
                            email = email
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
                        echannel = new
                        {
                            bill_info1 = "Payment For:",
                            bill_info2 = "debt"
                        } 
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
                            order_id = "Order"+"-"+Order_id+"BNI"+user_id,
                            gross_amount = total_cart
                        },
                        customer_details = new
                        {
                            email = email
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
                        payment_type = paymentMethod,
                        bank_transfer = new 
                        {
                            bank = bank_type,
                            permata = new 
                            {
                                recepient_name = firstName + lastName
                            }
                        },
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


        [Authorize]
        public IActionResult Transaksi()
        {
            var user_list = from l in AppDbContext.User where l.role == 1 select l;
            ViewBag.UL = user_list;
            var user_id = HttpContext.Session.GetString("Id");
            var pembayaran = from pur in AppDbContext.Purchase where pur.User_id == int.Parse(user_id) orderby pur.transaction_id select pur.Transaction_Details;
            var cart = (from c in AppDbContext.Transaksi where c.User_id == int.Parse(user_id) select c.Cart).Distinct();
            //var user_id = HttpContext.Session.GetString("Id");
            ViewBag.User = user_id;
            ViewBag.CartId = cart;
            ViewBag.Transaksi = pembayaran;
            var receive_chat = from l in AppDbContext.Chat where l.receiver_id == int.Parse(user_id) || l.sender_id == int.Parse(user_id) orderby l.created_at select l;
            var unreadchat = (from l in AppDbContext.Chat where l.read_at == DateTime.Parse("0001-01-01 00:00:00.0000000") && l.receiver_id == int.Parse(user_id) orderby l.created_at select l).Count();
            var sender_chat = from l in AppDbContext.Chat where l.sender_id == int.Parse(user_id) orderby l.created_at select l;
            //var user_id = HttpContext.Session.GetString("Id");
            ViewBag.UnRead = unreadchat;
            ViewBag.RChat = receive_chat;
            ViewBag.SChat = sender_chat;
            return View("Transaksi");
        }

        public async Task<IActionResult> Refresh()
        {
            var client = new HttpClient();
            var user_id = HttpContext.Session.GetString("Id");
            var pembayaran = (from v in AppDbContext.Purchase where v.User_id == int.Parse(user_id) orderby v.transaction_id descending select v.Transaction_Details);
           //var user_id = HttpContext.Session.GetString("Id");
            ViewBag.User = user_id;
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

        public string MailService(string email)
        {
            var user_id = HttpContext.Session.GetString("Id");
            var pembayaran = (from pur in AppDbContext.Purchase where pur.User_id == int.Parse(user_id) orderby pur.transaction_id descending select pur.Transaction_Details).FirstOrDefault(); 
            var va = (from v in AppDbContext.Purchase where v.User_id == int.Parse(user_id) orderby v.transaction_id descending select v.Transaction_Details.va_numbers).FirstOrDefault();
            MailAddress to = new MailAddress(email);
            MailAddress from = new MailAddress("hudafauzanm@gmail.com");
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Midtrans-Payment-Invoice";
            
            //HTML FORMAT
            foreach(var v in va)
            {
                 message.Body = $@"<p>Hey {email},<br>
                                <h3>INVOICE PEMBAYARAN</h3><br>
                                <p>Transaksi ID : {pembayaran.transaction_id}<br>
                                <p>Order ID : {pembayaran.order_id}<br>
                                <p>Status : {pembayaran.transaction_status}<br>
                                <p>Total Pembayaran : {pembayaran.gross_amount}<br>
                                <p>Bank : {v.bank}<br>
                                <h4> VA : {v.va_number} </h4><br>
                                <p>Thankyou<br>
                                <p>-- ADMIN<br>";
            }
           
            message.IsBodyHtml = true;
            //PLAIN FORMAT
            // foreach(var v in va)
            // {
            //      message.Body = "Transaksi ID : "+pembayaran.transaction_id + "\n"+"Order ID : " + pembayaran.order_id + "\n" +"Status : "+ pembayaran.transaction_status +"\n"+"Total Pembayaran : "+ pembayaran.gross_amount + "\n"+"Bank : "+v.bank+"\n"+"Virtual Account : " + v.va_number;
                 
            // }
            SmtpClient client = new SmtpClient("smtp.mailtrap.io", 587)
            {
                Credentials = new NetworkCredential("00d7115f88c37d", "2d3d930201d6ed"),
                EnableSsl = true
            };
            try
            {  
                client.Send(message);
                return ("success");
            }
            catch (SmtpException ex)
            {
            Console.WriteLine(ex.ToString());
            return ("gagal");
            }
         }    
    }
}