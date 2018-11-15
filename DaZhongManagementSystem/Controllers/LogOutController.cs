using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities;
using DaZhongManagementSystem.Entities.View;
using SyntacticSugar;

namespace DaZhongManagementSystem.Controllers
{
    public class LogOutController : Controller
    {
        //
        // GET: /LogOut/

        public ActionResult Index()
        {
            return View();
        }

        public string ProcessLogOut()
        {
            var cm = CookiesManager<V_User_Information>.GetInstance();
            cm.Remove(CostCookies.COOKIES_KEY_LOGIN);
            return "ok";
        }

    }
}
