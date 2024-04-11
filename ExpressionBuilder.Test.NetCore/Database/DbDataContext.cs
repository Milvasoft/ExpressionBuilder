using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpressionBuilder.Test.NetCore.Database;

public class DbDataContext : DbContext
{
    public DbSet<Products> Products { get; private set; }
    public DbSet<Orders> Orders { get; private set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite("Data Source=Northwind.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categories>(entity =>
        {
            entity.HasOne<Products>()
                .WithOne(c => c.Categories)
                .HasForeignKey<Categories>(f => f.CategoryID);
        });

        modelBuilder.Entity<OrderDetails>(entity =>
        {
            entity.HasKey(e => new { e.OrderID, e.ProductID });
            
            entity.HasOne(p => p.Products)
                .WithMany(c => c.OrderDetails)
                .HasForeignKey(c => c.ProductID);
            
            entity.HasOne(p => p.Orders);
        });
        base.OnModelCreating(modelBuilder);
    }
}

public class Products
{
    [Key]
    public int ProductID { get; set; }

    public bool Discontinued { get; set; }
    public decimal UnitPrice { get; set; }
    public int SupplierID { get; set; }
    public int CategoryID { get; set; }
    public Categories Categories { get; set; }
    public List<OrderDetails> OrderDetails { get; set; }
}

public class Categories
{
    [Key]
    public int CategoryID { get; set; }

    public string CategoryName { get; set; }
}

public class Orders
{
    [Key]
    public int OrderID { get; set; }

    public string ShipRegion { get; set; }
}

public class OrderDetails
{
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public Products Products { get; set; }
    public Orders Orders { get; set; }
    public float Discount { get; set; }
}