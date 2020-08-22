using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.DataHandler.Serializer;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using ReadLater.Data;
using ReadLater.Repository;
using ReadLater.Services;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using WebApi.Models;

namespace WebApi
{
    public partial class Startup
    {
        public static void ConfigureSimpleInjector(IAppBuilder app, Container container)
        {
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
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

            container.Register<ISecureDataFormat<AuthenticationTicket>, SecureDataFormat<AuthenticationTicket>>();
            container.Register<ITextEncoder, Base64UrlTextEncoder>();
            container.Register<IDataSerializer<AuthenticationTicket>, TicketSerializer>();
            container.Register<IDataProtector>(() => new DpapiDataProtectionProvider().Create("ASP.NET Identity"));

            container.Register<ICategoryService, CategoryService>(Lifestyle.Scoped);
            container.Register<IBookmarkService, BookmarkService>(Lifestyle.Scoped);
        }

    }
}