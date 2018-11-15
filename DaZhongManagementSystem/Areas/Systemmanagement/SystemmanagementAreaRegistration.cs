using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.Systemmanagement
{
    public class SystemmanagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Systemmanagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Systemmanagement_default",
                "Systemmanagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
