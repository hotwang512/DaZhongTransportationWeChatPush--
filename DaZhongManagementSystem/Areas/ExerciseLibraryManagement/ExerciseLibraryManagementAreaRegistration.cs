using System.Web.Mvc;

namespace DaZhongManagementSystem.Areas.ExerciseLibraryManagement
{
    public class ExerciseLibraryManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ExerciseLibraryManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ExerciseLibraryManagement_default",
                "ExerciseLibraryManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
