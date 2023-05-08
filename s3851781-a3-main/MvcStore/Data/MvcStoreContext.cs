using Microsoft.EntityFrameworkCore;
using MvcStore.Models;

namespace MvcStore.Data;

public class MvcStoreContext : DbContext
{
    public MvcStoreContext (DbContextOptions<MvcStoreContext> options) : base(options)
    { }

    public DbSet<Order> Order { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<OrderedProducts> OrderedProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Composite key for OrderedProducts to Order and Product
        modelBuilder.Entity<OrderedProducts>().HasKey(x => new { x.OrderID, x.ProductID });
    }
}
