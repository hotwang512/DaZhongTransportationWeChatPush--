using System.Web.Mvc;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities.TableEntity;
using SyntacticSugar;

namespace DaZhongManagementSystem.Controllers
{
    public class BaseController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var cm = CookiesManager<Sys_User>.GetInstance();
            if (!cm.ContainsKey(CostCookies.COOKIES_KEY_LOGIN))
            {
                Response.Redirect("/Login/Index");
            }
        }
    }
}