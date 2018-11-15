using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.KnowledgeBaseManagement
{
    public class KnowledgeBaseManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "KnowledgeBaseManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "KnowledgeBaseManagement_default",
                "KnowledgeBaseManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
