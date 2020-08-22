using MVC.Models;
using ReadLater.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class DashboardController : Controller
    {
        IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        // GET: Dashboard
        public ActionResult Index()
        {
            if (ClaimsPrincipal.Current.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier);
                var dashboard = new DashboardViewModel();
                dashboard.Categories = _dashboardService.GetDashboardCategoriesData(new Guid(userId.Value));
                dashboard.Bookmarks = _dashboardService.GetDashboardBookmarksData(new Guid(userId.Value));
                return View(dashboard);
            }
            return Redirect("Home/Index");
        }


        public ActionResult Details(int? categoryId)
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var details = _dashboardService.GetDashboardCategoryDetailsData((int)categoryId);

            if (details == null)
            {
                return HttpNotFound();
            }
            return View(details);
        }

        public ActionResult IndexSummary()
        {
            var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier);
            var dashboard = _dashboardService.GetDashboardBookmarksSummaryData();
            if (dashboard != null)
            {
                return View(dashboard);
            }
            return Redirect("Home/Index");
        }
    }
}