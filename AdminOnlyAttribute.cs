using System.Web;
using System.Web.Mvc;
namespace MaSV_LTW.Areas.Admin.Filters {
 public class AdminOnlyAttribute : AuthorizeAttribute {
  protected override bool AuthorizeCore(HttpContextBase context) { return context.Session != null && (context.Session["AdminUser"] as string) != null; }
  protected override void HandleUnauthorizedRequest(AuthorizationContext context) { context.Result = new RedirectResult("~/Admin/Account/Login"); }
 }
}
