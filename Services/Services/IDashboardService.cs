using ReadLater.Entities;
using System;
using System.Collections.Generic;

namespace ReadLater.Services
{
    public interface IDashboardService
    {
        List<Category> GetDashboardCategoriesData(Guid userId);
        List<Bookmark> GetDashboardBookmarksData(Guid userId);
        List<Bookmark> GetDashboardCategoryDetailsData(int categoryId);
        List<Bookmark> GetDashboardBookmarksSummaryData();
    }
}
