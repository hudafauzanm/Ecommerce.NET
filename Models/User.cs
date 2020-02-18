using System;
using System.Collections.Generic;

namespace Razor.Models
{
    public class User
    {
        public int id{get;set;}
        public string username{get;set;}
        public string email{get;set;}
        public string password{get;set;}
        public int role{get;set;}
        public DateTime created_at{get;set;}
        public DateTime published_at{get;set;}
        public ICollection<Transaksi> Transaksi{get;set;}
        public ICollection<Purchase> Purchases{get;set;}
    }
}