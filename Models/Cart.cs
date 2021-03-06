using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Razor.Models
{
    public class Cart
    {
        public int id{get;set;}
        public double total{get;set;}
        public DateTime created_at{get;set;}
        public DateTime published_at{get;set;}
        public ICollection<Transaksi> Transaksi{get;set;}
    }
}