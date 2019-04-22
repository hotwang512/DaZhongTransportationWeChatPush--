using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.RideCheckFeedback
{
    public class RideCheckFeedbackAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RideCheckFeedback";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RideCheckFeedback_default",
                "RideCheckFeedback/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
