using ReadLater.Entities;
using System.Collections.Generic;

namespace MVC.Models
{
    public class DashboardViewModel
    {
        public List<Bookmark> Bookmarks { get; set; }
        public List<Category> Categories { get; set; }
    }
}