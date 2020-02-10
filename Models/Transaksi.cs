namespace Razor.Models
{
    public class Transaksi
    {
      public int Item_id {get;set;}
      public int Cart_id{get;set;}
      public int User_id{get;set;}
      public Cart Cart{get;set;}
      public Item Item{get;set;}   
      public User User{get;set;}
    }
}