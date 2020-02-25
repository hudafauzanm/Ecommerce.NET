using System;
using System.ComponentModel.DataAnnotations;

namespace Razor.Models
{
    public class Notification
    {
        [Key]
        public Guid id{get;set;}
        public int sender_id{get;set;}
        public int receiver_id{get;set;}
        public int role_id{get;set;}
        public DateTime read_at{get;set;}
        public DateTime created_at{get;set;}
    }
}