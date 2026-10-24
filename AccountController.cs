using System;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using MaSV_LTW.Models;
namespace MaSV_LTW.Areas.Admin.Controllers {
 public class AccountController : Controller {
 private readonly MyStoreContext db=new MyStoreContext();
 public ActionResult Login(){return View();}
 [HttpPost,ValidateAntiForgeryToken] public ActionResult Login(string username,string password){
  if(String.IsNullOrWhiteSpace(username)||String.IsNullOrEmpty(password)){ModelState.AddModelError("","Nhập tên tài khoản và mật khẩu.");return View();}
  // The instructor SQL stores plain-text credentials; this is ONLY for classroom demonstration.
  var account=db.Users.FirstOrDefault(u=>u.Username==username && u.UserRole.Trim()=="A");
  if(account==null || account.Password.TrimEnd()!=password){ModelState.AddModelError("","Thông tin đăng nhập không đúng.");return View();}
  Session["AdminUser"]=account.Username;return RedirectToAction("Index","Home",new{area="Admin"});
 }
 [HttpPost,ValidateAntiForgeryToken] public ActionResult Logout(){Session.Remove("AdminUser");return RedirectToAction("Login");}
 [MaSV_LTW.Areas.Admin.Filters.AdminOnly] public ActionResult Index(string q,int page=1){var a=db.Users.Where(x=>x.UserRole.Trim()=="A");if(!String.IsNullOrWhiteSpace(q))a=a.Where(x=>x.Username.Contains(q));a=a.OrderBy(x=>x.Username);int size=10,count=a.Count();int pages=Math.Max(1,(count+size-1)/size);page=Math.Max(1,Math.Min(page,pages));ViewBag.Q=q;ViewBag.Page=page;ViewBag.Pages=pages;return View(a.Skip((page-1)*size).Take(size).ToList());}
 protected override void Dispose(bool d){if(d)db.Dispose();base.Dispose(d);}
 }}