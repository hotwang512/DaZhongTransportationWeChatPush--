using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DaZhongManagementSystem.Common;
using DaZhongManagementSystem.Entities;
using DaZhongManagementSystem.Models.Filters;
using SyntacticSugar;
using DaZhongManagementSystem.Entities.TableEntity;

namespace DaZhongManagementSystem
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ActionFilter());
            //filters.Add(new CheckLoginAttribute());//调用登录验证
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Login", action = "Index", id = UrlParameter.Optional } // 参数默认值
                //new string[] { "DaZhongManagementSystem.Controllers" }
            );
        }
        public void Application_BeginRequest(object sender, EventArgs e)
        {
            Global_Application_BeginRequest.Filter(HttpContext.Current);
        }


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // 默认情况下对 Entity Framework 使用 LocalDB
            Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            //从配置文件读取log4net配置，然后进行初始化
            log4net.Config.XmlConfigurator.Configure();
        }
    }


    /// <summary>
    /// 全局登录约束整体过滤
    /// </summary>
    public class CheckLoginAttribute : ActionFilterAttribute
    {
        public CheckLoginAttribute()
        {

        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {            
            var condesc = filterContext.ActionDescriptor.ControllerDescriptor;
            string actionName = filterContext.ActionDescriptor.ActionName.ToLower();
            var controllerName = condesc.ControllerName.ToLower();
            if (actionName == "index" && controllerName == "login")
            {
                return;
            }
            var cm = CookiesManager<Sys_User>.GetInstance();
            if (cm.ContainsKey(CostCookies.COOKIES_KEY_LOGIN))
            {
                Sys_User currentUser = cm[CostCookies.COOKIES_KEY_LOGIN];
                cm.Add(CostCookies.COOKIES_KEY_LOGIN, currentUser, cm.Hour * 24); //保存24小时
            }
            else
            {
                ErrorRedirect(filterContext, "index");
            }
        }
        /// <summary>
        /// 登录验证失败
        /// </summary>
        /// <param name="filterContext"></param>
        private void ErrorRedirect(ActionExecutingContext filterContext, string errorAction)
        {
            filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Login", action = errorAction }));
        }

    }
}