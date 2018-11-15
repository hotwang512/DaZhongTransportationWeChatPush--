using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.WeChatPay
{
    public class WeChatPayAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WeChatPay";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WeChatPay_default",
                "WeChatPay/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
