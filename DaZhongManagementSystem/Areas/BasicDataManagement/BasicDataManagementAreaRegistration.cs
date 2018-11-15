using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.BasicDataManagement
{
    public class BasicDataManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BasicDataManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "BasicDataManagement_default",
                "BasicDataManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
