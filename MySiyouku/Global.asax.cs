using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Mvc;
using MySiyouku.App_Start;
using Siyouku.Model;

namespace MySiyouku
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            //autofac
            IocConfig.InstallIoc();

            AutoMapperConfig.InstallAutoMapper();
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }
    }
}