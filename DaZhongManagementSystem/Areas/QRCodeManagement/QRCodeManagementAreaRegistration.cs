using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.QRCodeManagement
{
    public class QRCodeManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "QRCodeManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "QRCodeManagement_default",
                "QRCodeManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
