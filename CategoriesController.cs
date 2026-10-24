using System;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using MaSV_LTW.Models;
namespace MaSV_LTW.Areas.Admin.Controllers {
 [MaSV_LTW.Areas.Admin.Filters.AdminOnly] public class CategoriesController : Controller {
 private readonly MyStoreContext db=new MyStoreContext();
 public ActionResult Index() { return View(db.Categories.OrderBy(x=>x.CategoryID).ToList()); }
 public ActionResult Details(int? id) { if(!id.HasValue)return new HttpStatusCodeResult(HttpStatusCode.BadRequest);var x=db.Categories.Find(id);if(x==null)return HttpNotFound();return View(x); }
 public ActionResult Create() { return View(new Category()); }
 [HttpPost,ValidateAntiForgeryToken] public ActionResult Create([Bind(Include="CategoryName")] Category x) { if(ModelState.IsValid){db.Categories.Add(x);db.SaveChanges();return RedirectToAction("Index");}return View(x); }
 public ActionResult Edit(int? id) { if(!id.HasValue)return new HttpStatusCodeResult(400);var x=db.Categories.Find(id);if(x==null)return HttpNotFound();return View(x); }
 [HttpPost,ValidateAntiForgeryToken] public ActionResult Edit([Bind(Include="CategoryID,CategoryName")] Category x) { if(ModelState.IsValid){db.Entry(x).State=EntityState.Modified;db.SaveChanges();return RedirectToAction("Index");}return View(x); }
 public ActionResult Delete(int? id) { if(!id.HasValue)return new HttpStatusCodeResult(400);var x=db.Categories.Find(id);if(x==null)return HttpNotFound();return View(x); }
 [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken] public ActionResult DeleteConfirmed(int id) {var x=db.Categories.Find(id);if(x==null)return HttpNotFound();if(db.Products.Any(p=>p.CategoryID==id)){ModelState.AddModelError("","Danh mục còn sản phẩm, không thể xóa.");return View("Delete",x);}db.Categories.Remove(x);db.SaveChanges();return RedirectToAction("Index");}
 protected override void Dispose(bool d){if(d)db.Dispose();base.Dispose(d);}
 } }