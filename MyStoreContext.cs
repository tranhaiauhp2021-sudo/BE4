using System.Data.Entity;
namespace MaSV_LTW.Models {
 public class MyStoreContext : DbContext {
  public MyStoreContext() : base("name=MyStoreContext") { Database.SetInitializer<MyStoreContext>(null); }
  public DbSet<Category> Categories {get;set;}
  public DbSet<Product> Products {get;set;}
  public DbSet<StoreUser> Users {get;set;}
  public DbSet<Customer> Customers {get;set;}
  public DbSet<Order> Orders {get;set;}
  public DbSet<OrderDetail> OrderDetails {get;set;}
  protected override void OnModelCreating(DbModelBuilder modelBuilder) {
   modelBuilder.Entity<Product>().HasRequired(x=>x.Category).WithMany(x=>x.Products).HasForeignKey(x=>x.CategoryID).WillCascadeOnDelete(false);
   modelBuilder.Entity<Customer>().HasRequired(x=>x.User).WithMany().HasForeignKey(x=>x.Username).WillCascadeOnDelete(false);
   modelBuilder.Entity<Order>().HasRequired(x=>x.Customer).WithMany().HasForeignKey(x=>x.CustomerID).WillCascadeOnDelete(false);
   modelBuilder.Entity<OrderDetail>().HasRequired(x=>x.Order).WithMany().HasForeignKey(x=>x.OrderID).WillCascadeOnDelete(false);
   modelBuilder.Entity<OrderDetail>().HasRequired(x=>x.Product).WithMany().HasForeignKey(x=>x.ProductID).WillCascadeOnDelete(false);
   modelBuilder.Entity<Product>().Property(x=>x.ProductPrice).HasPrecision(18,2);
   modelBuilder.Entity<Order>().Property(x=>x.TotalAmount).HasPrecision(18,2);
   modelBuilder.Entity<OrderDetail>().Property(x=>x.UnitPrice).HasPrecision(18,2);
   base.OnModelCreating(modelBuilder);
  }
 }
}
