using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.SecondaryCleaningManagement
{
    public class SecondaryCleaningManagementAreaRegistration: AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SecondaryCleaningManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SecondaryCleaningManagement_default",
                "SecondaryCleaningManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}