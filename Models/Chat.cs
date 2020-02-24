using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Razor.Models
{
    public class Chat
    {
        [Key]
        public Guid id{get;set;}
        public int sender_id{get;set;}
        public int receiver_id{get;set;}
        [Column(TypeName="text")]
        public string message{get;set;}
        public DateTime read_at{get;set;}
        public DateTime created_at{get;set;}
    }
}