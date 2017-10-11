using System.Web.Mvc;

namespace MySiyouku.Areas.Manage
{
    public class ManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Manage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("Manage",
                             "Manage/",
     new { Controller = "MyHome", Action = "Login" });

            context.MapRoute(
                "Manage_default",
                "Manage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional ,
                    Namespaces = new[] { " MySiyouku.Areas.Manage.Controllers" } }
            );
        }
    }
}