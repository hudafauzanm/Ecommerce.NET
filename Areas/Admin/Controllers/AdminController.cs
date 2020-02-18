using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Razor.Data;
using Razor.Models;

namespace Razor.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        public AppDbContext AppDbContext {get;set;}

        public AdminController(IConfiguration configuration,AppDbContext appDbContext)
        {
            AppDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            // var token = HttpContext.Session.GetString("JWTToken");
            // var jwtSec  = new JwtSecurityTokenHandler();
            // var securityToken = jwtSec.ReadToken(token) as JwtSecurityToken;
            // var sub = securityToken?.Claims.First(Claim => Claim.Type == "sub").Value;
            var item = from l in AppDbContext.Item select l;
            ViewBag.Item = item;
            return View("Admin");        
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

          return RedirectToAction("Index","Admin");
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

          return RedirectToAction("Index","Admin");
        }

        public IActionResult Delete(int Id_item)
        {
          var x = AppDbContext.Item.Find(Id_item);
          AppDbContext.Remove(x);
          AppDbContext.SaveChanges();
          Console.WriteLine("Berhasil Hapus Data");

          return RedirectToAction("Index","Admin");
        }
        public IActionResult Import([FromForm(Name ="Upload")] IFormFile Upload)
        {
            string filePath = string.Empty;
            Console.WriteLine(Upload.FileName);
            if(Upload != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(Upload.FileName);
 
                    //Validate uploaded file and return error.
                    if (fileExtension != ".csv")
                    {
                        ViewBag.Message = "Please select the csv file with .csv extension";
                        Console.WriteLine("Bukan");
                        return RedirectToAction("Index","Admin");
                    }
                    using (var sreader = new StreamReader(Upload.OpenReadStream()))
                    {
                        string[] headers = sreader.ReadLine().Split(',');

                        while (!sreader.EndOfStream)
                        {
                            string[] rows = sreader.ReadLine().Split(',');
 
                            Item item = new Item()
                            {
                                item_name = rows[1].ToString(),
                                url_img = rows[2].ToString(),
                                price = double.Parse(rows[3].ToString()),
                                description = rows[4].ToString(),
                                rating = int.Parse(rows[5].ToString()),
                                created_at = DateTime.Now,
                                published_at = DateTime.Now
                            };

                            AppDbContext.Item.Add(item);
                        }
                        AppDbContext.SaveChanges();
                    }
                    return RedirectToAction("Index","Admin");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }
            }
            else
            {
                Console.WriteLine("GAGAL MASUK 1");
                ViewBag.Message = "Please select the file first to upload.";
            }
            Console.WriteLine("GAGAL MASUK 2");
            return RedirectToAction("Index","Admin");
        }

        [HttpPost]
        public FileResult Export()
        {
            var export_item = from ex in AppDbContext.Item select ex;
            string[] colomnama = new string[8] {"Item ID","Item Name","Url_Img","Price","Description","Rating","Created","Published"};
            string csv = string.Empty;

            foreach (string columnName in colomnama)
            {
                csv += columnName + ',';
            }

            csv += "\r\n";
 
            foreach (var item in export_item)
                {
                    Console.WriteLine(item);
                    //Add the Data rows.
                    csv += item.id;
                    csv += ",";
                    csv += item.item_name.Replace(",", ";") + ',';
                    csv += item.url_img.Replace(",", ";") + ',';
                    csv += item.price;
                    csv += ",";
                    csv += item.description.Replace(",", ";") + ',';
                    csv += item.rating;
                    csv += ",";
                    csv += item.created_at;
                    csv += ",";
                    csv += item.published_at;
        
                    //Add new line.
                    csv += "\r\n";
                }
        
                //Download the CSV file.
                byte[] bytes = Encoding.ASCII.GetBytes(csv);
                return File(bytes, "application/text", "Item.csv");
            }   
    }
}