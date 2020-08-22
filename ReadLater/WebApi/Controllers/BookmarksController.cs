using ReadLater.Entities;
using ReadLater.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Controllers
{
    [Authorize]
    public class BookmarksController : ApiController
    {
        ICategoryService _categoryService;
        IBookmarkService _bookmarkService;
        public BookmarksController(ICategoryService categoryService, IBookmarkService bookmarkService)
        {
            _categoryService = categoryService;
            _bookmarkService = bookmarkService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Index(string category)
        {
            if (ClaimsPrincipal.Current.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier);
                List<Bookmark> model = _bookmarkService.GetBookmarks(category, new Guid(userId.Value));
                return Ok(model);
            }
            else
            {
                return Unauthorized();
            }
        }

        // GET: Bookmarks/Details/5
        [HttpGet]
        public async Task<IHttpActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest("Bad Request");
            }
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            if (bookmark == null)
            {
                BadRequest("Could not find this bookmark");
            }
            return Ok(bookmark);

        }

        // GET: Categories/Create
        [HttpGet]
        public async Task<IHttpActionResult> Create()
        {
            //if (ClaimsPrincipal.Current.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            //{
            //    var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier);
            //    //ViewData["Categories"] = new SelectList(_categoryService.GetCategories(new Guid(userId.Value)).ToList(), "ID", "Name");
            //}
            var bookmark = new Bookmark();
            return Ok(bookmark);
        }

        // POST: Bookmarks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody] Bookmark bookmark)
        {
            if (ModelState.IsValid)
            {
                if (ClaimsPrincipal.Current.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
                {
                    var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier);
                    _bookmarkService.CreateBookmark(bookmark, new Guid(userId.Value));
                    return Redirect("Index");
                }
            }

            return Ok(bookmark);
        }

        // GET: Bookmarks/Edit/5
        [HttpGet]
        public async Task<IHttpActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest("Bad request");
            }
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            if (bookmark == null)
            {
                BadRequest("Could not find this bookmark");
            }
            return Ok(bookmark);
        }

        // POST: Bookmarks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IHttpActionResult> Edit([FromBody] Bookmark bookmark)
        {
            if (ModelState.IsValid)
            {
                _bookmarkService.UpdateBookmark(bookmark);
                return Ok(bookmark);
            }
            return BadRequest("The model is not valid");
        }

        // GET: Bookmarks/Delete/5
        [HttpGet]
        public async Task<IHttpActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("Id is null");
            }
            Bookmark bookmark = _bookmarkService.GetBookmark((int)id);
            if (bookmark == null)
            {
                return BadRequest("A Bookmark with that Id doesn't exist");
            }
            return Ok(bookmark);
        }

        // POST: Bookmarks/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IHttpActionResult> DeleteConfirmed(int id)
        {
            Bookmark bookmark = _bookmarkService.GetBookmark(id);
            _bookmarkService.DeleteBookmark(bookmark);
            return Redirect("Index");
        }

        [HttpGet, ActionName("Clicked")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Clicked(int? id)
        {
            if (id == null)
            {
                return BadRequest("Id is null");
            }
            var bookmark = _bookmarkService.BookmarkClicked((int)id);

            return Redirect(bookmark.URL);
        }
    }
}