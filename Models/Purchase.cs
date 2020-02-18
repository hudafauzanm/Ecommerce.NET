using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
// using System.Text.Json;
// using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Razor.Models
{
    public class Purchase
    {
      public int id{get;set;}
      public string nama_pemesan{get;set;}
      public string email{get;set;}
      public string alamat{get;set;}
      public string kodepos{get;set;}
      public DateTime created_at{get;set;}
      public DateTime published_at{get;set;}
      [ForeignKey("Transaction_Details")]
      public int transaction_id{get;set;}
      public Transaction_Details Transaction_Details{get;set;}

      [ForeignKey("User")]
      public int User_id{get;set;}
      public User User{get;set;}  
    }

    public class Transaction_Details
    {
      [JsonIgnore]
      public int id{get;set;}
      public string transaction_id{get;set;}
      public string merchant_id{get;set;}
      public string order_id{get;set;}
      public string currency{get;set;}
      public string gross_amount{get;set;}
      public string transaction_time{get;set;}
      public string transaction_status{get;set;}
      public string status_code{get;set;}
      public string status_message{get;set;}
      public string fraud_status{get;set;}
      public string signature_key{get;set;}
      public string _account{get;set;}
      public string _actions{get;set;}

      [NotMapped]
      public List<Account> va_numbers
      {
        get{return _account == null ? null : JsonConvert.DeserializeObject<List<Account>>(_account);}
        set{
          _account = JsonConvert.SerializeObject(value);
          Console.WriteLine(_account);  
        }
      }

      [NotMapped]
      public List<ActionModel> actions
      {
        get{return _actions == null ? null : JsonConvert.DeserializeObject<List<ActionModel>>(_actions);}
        set{_actions = JsonConvert.SerializeObject(value);}
      }
    }

    public class Account
    {
      public string bank{get;set;}
      public string va_number{get;set;}
    }

    public class ActionModel 
    {
      public string method{get;set;}
      public string url{get;set;}
    }
}