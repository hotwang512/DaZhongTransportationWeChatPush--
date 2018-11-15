using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.WeChatPush
{
    public class WeChatPushAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WeChatPush";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WeChatPush_default",
                "WeChatPush/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
