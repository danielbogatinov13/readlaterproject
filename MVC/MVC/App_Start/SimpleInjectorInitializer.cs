using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using MVC.Models;
using Owin;
using ReadLater.Data;
using ReadLater.Repository;
using ReadLater.Services;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MVC
{
    public partial class Startup
    {
        public static void ConfigureSimpleInjector(IAppBuilder app, Container container)
        {
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
        private static void InitializeContainer(Container container)
        {
            // For instance:
            // container.Register<IUserRepository, SqlUserRepository>();

            container.Register<IDbContext, ReadLaterDataContext>(Lifestyle.Scoped);
            container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);

            container.Register<ApplicationDbContext>(() => new ApplicationDbContext(), Lifestyle.Scoped);
            container.Register<IAuthenticationManager>(() =>
                container.IsVerifying ? new OwinContext(new Dictionary<string, object>()).Authentication : HttpContext.Current.GetOwinContext().Authentication,
                Lifestyle.Scoped);
            container.Register<UserManager<ApplicationUser, string>, ApplicationUserManager>(Lifestyle.Scoped);
            container.Register<SignInManager<ApplicationUser, string>, ApplicationSignInManager>(Lifestyle.Scoped);
            container.Register<IUserStore<ApplicationUser>>(() => new UserStore<ApplicationUser>(container.GetInstance<ApplicationDbContext>()), Lifestyle.Scoped);
            container.Register<ApplicationUserManager>(Lifestyle.Scoped);
            container.Register<ApplicationSignInManager>(Lifestyle.Scoped);

            //services

            container.Register<ICategoryService, CategoryService>(Lifestyle.Scoped);
            container.Register<IBookmarkService, BookmarkService>(Lifestyle.Scoped);
            container.Register<IDashboardService, DashboardService>(Lifestyle.Scoped);
        }

    }
}