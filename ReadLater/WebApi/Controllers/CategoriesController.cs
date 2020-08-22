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
    public class CategoriesController : ApiController
    {
        ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Index()
        {
            if (ClaimsPrincipal.Current.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier);
                List<Category> model = _categoryService.GetCategories(new Guid(userId.Value));
                return Ok(model);
            }
            else
            {
                return Unauthorized();
            }
        }

        // GET: Categories/Details/5
        [HttpGet]
        public async Task<IHttpActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest("Bad Request");
            }
            Category category = _categoryService.GetCategory((int)id);
            if (category == null)
            {
                BadRequest("Could not find this category");
            }
            return Ok(category);

        }

        // GET: Categories/Create
        [HttpGet]
        public async Task<IHttpActionResult> Create()
        {
            var category = new Category();
            return Ok(category);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                if (ClaimsPrincipal.Current.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
                {
                    var userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier);
                    _categoryService.CreateCategory(category, new Guid(userId.Value));
                    return Redirect("Index");
                }
            }

            return Ok(category);
        }

        // GET: Categories/Edit/5
        [HttpGet]
        public async Task<IHttpActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest("Bad request");
            }
            Category category = _categoryService.GetCategory((int)id);
            if (category == null)
            {
                BadRequest("Could not find this category");
            }
            return Ok(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IHttpActionResult> Edit([FromBody] Category category)
        {
            //    if (ModelState.IsValid)
            //    {
            _categoryService.UpdateCategory(category);
            return Ok(category);
            //}
            return BadRequest("The model is not valid");
        }

        // GET: Categories/Delete/5
        [HttpGet]
        public async Task<IHttpActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest("Id is null");
            }
            Category category = _categoryService.GetCategory((int)id);
            if (category == null)
            {
                return BadRequest("A Category with that Id doesn't exist");
            }
            return Ok(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IHttpActionResult> DeleteConfirmed(int id)
        {
            Category category = _categoryService.GetCategory(id);
            _categoryService.DeleteCategory(category);
            return Redirect("Index");
        }
    }
}
