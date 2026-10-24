using System;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using MaSV_LTW.Models;
namespace MaSV_LTW.Areas.Admin.Controllers {
 [MaSV_LTW.Areas.Admin.Filters.AdminOnly] public class OrdersController : Controller {
 private readonly MyStoreContext db=new MyStoreContext();
 public ActionResult Index(int? customerId){var query=db.Orders.Include(o=>o.Customer).AsQueryable();if(customerId.HasValue)query=query.Where(o=>o.CustomerID==customerId);return View(query.OrderByDescending(o=>o.OrderDate).ThenByDescending(o=>o.OrderID).ToList());}
 public ActionResult Details(int? id){if(!id.HasValue)return new HttpStatusCodeResult(400);var x=db.Orders.Include(o=>o.Customer).Include(o=>o.OrderDetails.Select(d=>d.Product)).FirstOrDefault(o=>o.OrderID==id);if(x==null)return HttpNotFound();return View(x);}
 protected override void Dispose(bool d){if(d)db.Dispose();base.Dispose(d);}
 } }