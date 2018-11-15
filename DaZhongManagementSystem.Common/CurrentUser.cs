using DaZhongManagementSystem.Entities.TableEntity;
using SyntacticSugar;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;


namespace DaZhongManagementSystem.Common
{
    public class CurrentUser
    {
        public static Sys_User GetCurrentUser()
        {
            var cm = CookiesManager<Sys_User>.GetInstance();
            if (cm.ContainsKey(CostCookies.COOKIES_KEY_LOGIN))
            {
                return cm[CostCookies.COOKIES_KEY_LOGIN];
            }
            else
            {
                return null;
            }
        }
    }
}
