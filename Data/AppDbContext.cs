using Microsoft.EntityFrameworkCore;
using Razor.Models;

namespace Razor.Data
{
    public class AppDbContext : DbContext
    {
       public DbSet<Cart> Cart {get;set;}
       public DbSet<Item> Item {get;set;}
       public DbSet<Transaksi> Transaksi {get;set;}

       public AppDbContext(DbContextOptions options) : base (options)
       {
        
       }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Transaksi>()
                    .HasKey(t => new { t.Item_id, t.Cart_id });

                modelBuilder.Entity<Transaksi>()
                    .HasOne(pt => pt.Item)
                    .WithMany(p => p.Transaksi)
                    .HasForeignKey(pt => pt.Item_id);

                modelBuilder.Entity<Transaksi>()
                    .HasOne(pt => pt.Cart)
                    .WithMany(t => t.Transaksi)
                    .HasForeignKey(pt => pt.Cart_id);
            }
          
    }
}