using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Razor.Models
{
    public class Cart
    {
        public int id{get;set;}
        public double total{get;set;}
        public ICollection<Transaksi> Transaksi{get;set;}
    }
}