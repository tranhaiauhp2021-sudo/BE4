using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MaSV_LTW.Models {
 [Table("Category")] public class Category {
  [Key] public int CategoryID {get;set;}
  [Required] public string CategoryName {get;set;}
  public virtual ICollection<Product> Products {get;set;} = new HashSet<Product>();
 }
 [Table("Product")] public class Product {
  [Key] public int ProductID {get;set;}
  public int CategoryID {get;set;}
  [Required] public string ProductName {get;set;}
  [Required] public string ProductDecription {get;set;} // spelling is from lecturer's SQL schema
  public decimal ProductPrice {get;set;}
  public string ProductImage {get;set;}
  public virtual Category Category {get;set;}
 }
 [Table("User")] public class StoreUser {
  [Key, StringLength(255)] public string Username {get;set;}
  [Required, StringLength(50)] public string Password {get;set;}
  [Required, StringLength(1)] public string UserRole {get;set;}
 }
 [Table("Customer")] public class Customer {
  [Key] public int CustomerID {get;set;}
  [Required] public string CustomerName {get;set;}
  [Required] public string CustomerPhone {get;set;}
  public string CustomerEmail {get;set;}
  public string CustomerAddress {get;set;}
  [Required, StringLength(255)] public string Username {get;set;}
  public virtual StoreUser User {get;set;}
 }
 [Table("Order")] public class Order {
  [Key] public int OrderID {get;set;}
  public int CustomerID {get;set;}
  public DateTime OrderDate {get;set;}
  public decimal TotalAmount {get;set;}
  public string PaymentStatus {get;set;}
  [Required] public string AddressDelivery {get;set;}
  public virtual Customer Customer {get;set;}
  public virtual ICollection<OrderDetail> OrderDetails {get;set;} = new HashSet<OrderDetail>();
 }
 [Table("OrderDetail")] public class OrderDetail {
  [Key] public int ID {get;set;}
  public int ProductID {get;set;}
  public int OrderID {get;set;}
  public int Quantity {get;set;}
  public decimal UnitPrice {get;set;}
  public virtual Product Product {get;set;}
  public virtual Order Order {get;set;}
 }
}
