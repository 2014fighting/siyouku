using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Siyouku.Model;
using Siyouku.Repositorys;

namespace MySiyouku.App_Start
{
    public class IocConfig
    {
        public static void InstallIoc()
        {
            //ContainerBuilder builder = new ContainerBuilder();//容器构造器  组件中的类型通过此对象注册到容器中
            //builder.RegisterType<DbFactory>();//注册类型 这种简单较常用，但缺点是注册的类型必须在当前项目或被当前项目引用，因为使用泛型，必须类型明确。
            //builder.RegisterType(Type.GetType("Siyouku.Model.DbFactory"));//注册类型  针对上一种情况，还有一种通过Type对象进行注册的方式：
            //builder.RegisterType<Worker>().As<IPerson>();//注册类型且用as方法指定此类型是IPerson接口

            //var assembly = Assembly.GetExecutingAssembly();
            //var builder = new ContainerBuilder();
            //builder.RegisterAssemblyTypes(assembly)
            //     .Where(type => type.Name.EndsWith("Controller")); //条件判断
            // builder.RegisterType<SiyoukuContext>().As<IDbContext>();//注册类型且用as方法指定此类型是IPerson接口



            var assembly = Assembly.GetExecutingAssembly();
            var builder = new ContainerBuilder();

            // builder.RegisterInstance(new SiyoukuContext()).SingleInstance();//注册单例类
            builder.RegisterInstance(DbFactory.GetCurrentDbContext()).ExternallyOwned();//将自己系统中原有的单例注册为容器托管的单例
            builder.RegisterControllers(assembly);//注册当前执行代码程序集所有controllers
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            
            //注册仓储层
            var repositorys = Assembly.Load("Siyouku.Repositorys");
            builder.RegisterAssemblyTypes(repositorys)
                 .Where(type => type.Name.EndsWith("Repository")).AsImplementedInterfaces();


            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}