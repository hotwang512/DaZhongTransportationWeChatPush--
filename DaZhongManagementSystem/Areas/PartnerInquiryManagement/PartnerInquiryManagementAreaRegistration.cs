using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.PartnerInquiryManagement
{
    public class PartnerInquiryManagementAreaRegistration: AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PartnerInquiryManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PartnerInquiryManagement_default",
                "PartnerInquiryManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}