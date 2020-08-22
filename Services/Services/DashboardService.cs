using ReadLater.Entities;
using ReadLater.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadLater.Services
{
    public class DashboardService : IDashboardService
    {
        protected IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Category> GetDashboardCategoriesData(Guid userId)
        {
            var categories = _unitOfWork.Repository<Category>().Query()
                .Filter(x => x.UserCreatedId == userId)
                .OrderBy(l => l.OrderByDescending(b => b.Bookmarks.OrderByDescending(x => x.ClickCount).Sum(x => x.ClickCount)))
                .Get()
                .ToList();

            foreach (var sortingCategory in categories)
            {
                sortingCategory.Bookmarks = sortingCategory.Bookmarks.OrderByDescending(x => x.ClickCount).Take(5).ToList();
            }
            return categories;
        }

        public List<Bookmark> GetDashboardBookmarksData(Guid userId)
        {
            var bookmarks = _unitOfWork.Repository<Bookmark>().Query()
                .Filter(x => x.UserCreatedId == userId)
                .OrderBy(l => l.OrderByDescending(b => b.ClickCount))
                .Get()
                .Take(10)
                .ToList();


            return bookmarks;
        }

        public List<Bookmark> GetDashboardCategoryDetailsData(int categoryId)
        {
            var bookmarks = _unitOfWork.Repository<Bookmark>().Query()
                .Filter(x => x.CategoryId == categoryId)
                .OrderBy(l => l.OrderByDescending(b => b.ClickCount))
                .Get()
                .Take(10)
                .ToList();

            return bookmarks;
        }

        public List<Bookmark> GetDashboardBookmarksSummaryData()
        {
            var bookmarks = _unitOfWork.Repository<Bookmark>().Query()
                .OrderBy(l => l.OrderByDescending(b => b.ClickCount))
                .Get()
                .Take(10)
                .ToList();
            return bookmarks;
        }
    }
}
