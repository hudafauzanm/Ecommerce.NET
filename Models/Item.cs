using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Razor.Models
{
    public class Item
    {
        public int id{get;set;}
        public string item_name{get;set;}
        public string url_img{get;set;}
        public double price{get;set;}
        public string description{get;set;}
        public int rating {get;set;}
        public ICollection<Transaksi> Transaksi{get;set;}
    }
}