using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.DailyLogManagement
{
    public class DailyLogManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DailyLogManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DailyLogManagement_default",
                "DailyLogManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
