using ReadLater.Entities;
using ReadLater.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class BookmarksController : Controller
    {
        IBookmarkService _bookmarkService;
        ICategoryService _categoryService;
        public BookmarksController(IBookmarkService bookmarkService, ICategoryService categoryService)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
        }
        // GET: Bookmarks/{category}
        public ActionResult Index(string category)
        {
            if (ClaimsPrincipal.Current.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier);
                List<Bookmark> model = _bookmarkService.GetBookmarks(category, new Guid(userId.Value));
                return View(model);
            }
            else
            {
                return View();
            }
        }

        // GET: Bookmarks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            if (bookmark == null)
            {
                return HttpNotFound();
            }
            return View(bookmark);

        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            if (ClaimsPrincipal.Current.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier);
                ViewData["Categories"] = new SelectList(_categoryService.GetCategories(new Guid(userId.Value)).ToList(), "ID", "Name");
            }
            return View();
        }

        // POST: Bookmarks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,URL,ShortDescription,Category,CategoryId")] Bookmark bookmark)
        {
            if (ModelState.IsValid)
            {
                if (ClaimsPrincipal.Current.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
                {
                    var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier);
                    _bookmarkService.CreateBookmark(bookmark, new Guid(userId.Value));
                    return RedirectToAction("Index");
                }
            }

            return View(bookmark);
        }

        // GET: Bookmarks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            if (bookmark == null)
            {
                return HttpNotFound();
            }
            return View(bookmark);
        }

        // POST: Bookmarks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,URL,ShortDescription")] Bookmark bookmark)
        {
            if (ModelState.IsValid)
            {
                _bookmarkService.UpdateBookmark(bookmark);
                return RedirectToAction("Index");
            }
            return View(bookmark);
        }

        // GET: Bookmarks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            if (bookmark == null)
            {
                return HttpNotFound();
            }
            return View(bookmark);
        }

        // POST: Bookmarks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bookmark bookmark = _bookmarkService.GetBookmark(id);
            _bookmarkService.DeleteBookmark(bookmark);
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("Clicked")]
        public ActionResult Clicked(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             var bookmark =_bookmarkService.BookmarkClicked((int)id);

            return Redirect(bookmark.URL);

        }
    }
}