using System;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Mvc;
using MaSV_LTW.Models;
namespace MaSV_LTW.Areas.Admin.Controllers {
 [MaSV_LTW.Areas.Admin.Filters.AdminOnly] public class ProductsController : Controller {
 private readonly MyStoreContext db=new MyStoreContext();
 // BE4: Search + Sort + Pagination. Keep filters when navigating pages.
 public ActionResult Index(string q, decimal? minPrice, decimal? maxPrice,
   string sort = "name", int page = 1) {
   const int pageSize = 10;
   q = (q ?? "").Trim();
   if (minPrice.HasValue && minPrice.Value < 0) minPrice = 0;
   if (maxPrice.HasValue && maxPrice.Value < 0) maxPrice = 0;
   var query = db.Products.Include(x => x.Category).AsQueryable();
   if (!String.IsNullOrWhiteSpace(q)) {
     query = query.Where(x => x.ProductName.Contains(q)
       || x.ProductDecription.Contains(q)
       || x.Category.CategoryName.Contains(q));
   }
   if (minPrice.HasValue) query = query.Where(x => x.ProductPrice >= minPrice.Value);
   if (maxPrice.HasValue) query = query.Where(x => x.ProductPrice <= maxPrice.Value);
   switch (sort) {
     case "name_desc": query = query.OrderByDescending(x => x.ProductName).ThenBy(x => x.ProductID); break;
     case "price": query = query.OrderBy(x => x.ProductPrice).ThenBy(x => x.ProductID); break;
     case "price_desc": query = query.OrderByDescending(x => x.ProductPrice).ThenBy(x => x.ProductID); break;
     default: sort = "name"; query = query.OrderBy(x => x.ProductName).ThenBy(x => x.ProductID); break;
   }
   int totalItems = query.Count();
   int totalPages = Math.Max(1, (int)Math.Ceiling(totalItems / (double)pageSize));
   page = Math.Max(1, Math.Min(page, totalPages));
   var vm = new MaSV_LTW.ViewModels.ProductListViewModel {
     Products = query.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
     Search = q, Sort = sort, MinPrice = minPrice, MaxPrice = maxPrice,
     Page = page, TotalPages = totalPages, TotalItems = totalItems, PageSize = pageSize
   };
   return View(vm);
 }
 public ActionResult Details(int? id) {if(!id.HasValue)return new HttpStatusCodeResult(400);var x=db.Products.Include(p=>p.Category).FirstOrDefault(p=>p.ProductID==id.Value);if(x==null)return HttpNotFound();return View(x);}
 void LoadCategories(object selected=null) {ViewBag.CategoryID=new SelectList(db.Categories.OrderBy(c=>c.CategoryName),"CategoryID","CategoryName",selected);}
 // BE3: Category + Product, form dung lai Partial View va upload anh san pham.
 private string SaveProductImage(HttpPostedFileBase image) {
  if(image==null || image.ContentLength==0)return null;
  if(image.ContentLength>5*1024*1024)throw new InvalidOperationException("Ảnh không được vượt quá 5 MB.");
  var ext=Path.GetExtension(image.FileName).ToLowerInvariant();
  if(ext!=".jpg"&&ext!=".jpeg"&&ext!=".png"&&ext!=".webp")throw new InvalidOperationException("Chỉ nhận ảnh JPG, PNG hoặc WebP.");
  var folder=Server.MapPath("~/Content/images");Directory.CreateDirectory(folder);
  var filename=Guid.NewGuid().ToString("N")+ext;
  image.SaveAs(Path.Combine(folder,filename));
  return "/Content/images/"+filename;
 }
 public ActionResult Create(){LoadCategories();return View(new Product());}
 [HttpPost,ValidateAntiForgeryToken] public ActionResult Create([Bind(Include="CategoryID,ProductName,ProductDecription,ProductPrice")] Product x,HttpPostedFileBase imageFile){
  if(!db.Categories.Any(c=>c.CategoryID==x.CategoryID))ModelState.AddModelError("CategoryID","Chọn danh mục hợp lệ.");
  if(x.ProductPrice<0)ModelState.AddModelError("ProductPrice","Giá phải không âm.");
  if(ModelState.IsValid){try { x.ProductImage=SaveProductImage(imageFile);db.Products.Add(x);db.SaveChanges();return RedirectToAction("Index"); }
   catch(InvalidOperationException ex){ModelState.AddModelError("imageFile",ex.Message);} }
  LoadCategories(x.CategoryID);return View(x);
 }
 public ActionResult Edit(int? id){if(!id.HasValue)return new HttpStatusCodeResult(400);var x=db.Products.Find(id);if(x==null)return HttpNotFound();LoadCategories(x.CategoryID);return View(x);}
 [HttpPost,ValidateAntiForgeryToken] public ActionResult Edit(int id,[Bind(Include="CategoryID,ProductName,ProductDecription,ProductPrice")] Product input,HttpPostedFileBase imageFile){
  var current=db.Products.Find(id);if(current==null)return HttpNotFound();
  if(!db.Categories.Any(c=>c.CategoryID==input.CategoryID))ModelState.AddModelError("CategoryID","Chọn danh mục hợp lệ.");
  if(input.ProductPrice<0)ModelState.AddModelError("ProductPrice","Giá phải không âm.");
  if(ModelState.IsValid){try { var newImage=SaveProductImage(imageFile);current.CategoryID=input.CategoryID;current.ProductName=input.ProductName;current.ProductDecription=input.ProductDecription;current.ProductPrice=input.ProductPrice;
   if(newImage!=null)current.ProductImage=newImage;db.SaveChanges();return RedirectToAction("Index"); }
   catch(InvalidOperationException ex){ModelState.AddModelError("imageFile",ex.Message);} }
  input.ProductID=id;input.ProductImage=current.ProductImage;LoadCategories(input.CategoryID);return View(input);
 }
 public ActionResult Delete(int? id){if(!id.HasValue)return new HttpStatusCodeResult(400);var x=db.Products.Find(id);if(x==null)return HttpNotFound();return View(x);}
 [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken] public ActionResult DeleteConfirmed(int id){var x=db.Products.Find(id);if(x==null)return HttpNotFound();if(db.OrderDetails.Any(d=>d.ProductID==id)){ModelState.AddModelError("","Sản phẩm thuộc đơn hàng, không thể xóa.");return View("Delete",x);}db.Products.Remove(x);db.SaveChanges();return RedirectToAction("Index");}
 protected override void Dispose(bool d){if(d)db.Dispose();base.Dispose(d);}
 } }