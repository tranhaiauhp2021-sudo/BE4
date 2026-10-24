using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using MaSV_LTW.Models;
using MaSV_LTW.ViewModels;
namespace MaSV_LTW.Controllers {
 public class HomeController : Controller {
  private readonly MyStoreContext db = new MyStoreContext();
  public ActionResult Index(string search, string sort = "name", int page = 1) {
   var query = db.Products.Include(p=>p.Category).AsQueryable();
   if(!string.IsNullOrWhiteSpace(search)) query = query.Where(p=>p.ProductName.Contains(search));
   switch(sort) {
    case "price_asc": query = query.OrderBy(p=>p.ProductPrice).ThenBy(p=>p.ProductID); break;
    case "price_desc": query = query.OrderByDescending(p=>p.ProductPrice).ThenBy(p=>p.ProductID); break;
    default: sort="name"; query = query.OrderBy(p=>p.ProductName).ThenBy(p=>p.ProductID); break;
   }
   const int pageSize=8;
   int total=query.Count();
   int pages=Math.Max(1,(int)Math.Ceiling(total/(double)pageSize));
   page=Math.Max(1,Math.Min(page,pages));
   return View(new ProductListViewModel { Products=query.Skip((page-1)*pageSize).Take(pageSize).ToList(), Search=search, Sort=sort, Page=page, TotalPages=pages });
  }
  public ActionResult Detail(int id) { var product=db.Products.Include(p=>p.Category).FirstOrDefault(p=>p.ProductID==id); if(product==null) return HttpNotFound(); return View(product); }
  protected override void Dispose(bool disposing) { if(disposing) db.Dispose(); base.Dispose(disposing); }
 }
}
