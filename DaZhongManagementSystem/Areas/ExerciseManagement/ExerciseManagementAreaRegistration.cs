using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.ExerciseManagement
{
    public class ExerciseManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ExerciseManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ExerciseManagement_default",
                "ExerciseManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
