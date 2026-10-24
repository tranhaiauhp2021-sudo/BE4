using System;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using MaSV_LTW.Models;
namespace MaSV_LTW.Areas.Admin.Controllers {
 [MaSV_LTW.Areas.Admin.Filters.AdminOnly] public class CustomersController : Controller {
 private readonly MyStoreContext db=new MyStoreContext();
 public ActionResult Index(string q,string sort="name",int page=1){var a=db.Customers.AsQueryable();if(!String.IsNullOrWhiteSpace(q))a=a.Where(x=>x.CustomerName.Contains(q)||x.CustomerPhone.Contains(q)||x.Username.Contains(q));a=sort=="name_desc"?a.OrderByDescending(x=>x.CustomerName):a.OrderBy(x=>x.CustomerName);int size=10,count=a.Count();int pages=Math.Max(1,(count+size-1)/size);page=Math.Max(1,Math.Min(page,pages));ViewBag.Page=page;ViewBag.Pages=pages;ViewBag.Q=q;ViewBag.Sort=sort;return View(a.Skip((page-1)*size).Take(size).ToList());}
 public ActionResult Details(int? id){if(!id.HasValue)return new HttpStatusCodeResult(400);var x=db.Customers.Include(c=>c.User).FirstOrDefault(c=>c.CustomerID==id);if(x==null)return HttpNotFound();ViewBag.Orders=db.Orders.Where(o=>o.CustomerID==x.CustomerID).OrderByDescending(o=>o.OrderDate).ToList();return View(x);}
 protected override void Dispose(bool d){if(d)db.Dispose();base.Dispose(d);}
 } }