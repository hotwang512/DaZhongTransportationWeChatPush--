using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.QuestionManagement
{
    public class QuestionManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "QuestionManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "QuestionManagement_default",
                "QuestionManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}