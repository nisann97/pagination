using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.ViewModels.Blogs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Services.Interfaces;
using test.ViewModels;
using test.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace test.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBlogService _blogService;

        public BlogController(AppDbContext context, IBlogService blogService)
        {
            _context = context;
            _blogService = blogService;
            
        }

        public async Task<IActionResult> Index()
        {
            //int count = await _context.Blogs.CountAsync();
            //ViewBag.count = count;
            //return View(await _context.Blogs.Where(m => !m.SoftDeleted).ToListAsync());

            return View(await _blogService.GetAllOrderByDescAsync());
        }


        [HttpGet]
        public async Task<IActionResult> ShowMore(int skip)
        {
            List<Blog> blogs = await _context.Blogs.Skip(skip).Take(3).ToListAsync();
            
            return PartialView("_BlogsPartial", blogs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM blogCreate)
        {
            if (ModelState.IsValid)
            {
                return View();
            }

            if (ModelState.IsValid)
            {
                return View();
            }

            bool existBlog = await _blogService.ExistAsync(blogCreate.Title, blogCreate.Description);

            if (existBlog)
            {
                ModelState.AddModelError("Title", "Blog with this name already exist");
                ModelState.AddModelError("Description", "Blog with this description already exists");
                return View();
            }

            _blogService.CreateAsync(blogCreate);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Blog blog = await _blogService.GetByIdAsync((int) id);

            if (blog == null) return NotFound();

            BlogDetailVM model = new()
            {
                Title = blog.Title,
                Description = blog.Description,
                Date = blog.Date,
                Image = blog.Image
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            Blog blog = await _blogService.GetByIdAsync((int)id);
            if (blog == null) return NotFound();

            await _blogService.DeleteAsync(blog);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Blog blog = await _blogService.GetByIdAsync((int)id);
            if (blog is null) return NotFound();

            return View(new BlogEditVM { Id = blog.Id, Title = blog.Title, Description = blog.Description, Date = blog.Date });


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BlogEditVM blogEdit)
        {
            if (id is null) return BadRequest();

            Blog dbBlog = await _blogService.GetByIdAsync((int)id);
            if (dbBlog is null) return NotFound();

            bool existBlog = await _blogService.ExistForTitleAsync(blogEdit.Title);

            if (existBlog)
            {
                ModelState.AddModelError("Title", "Such blog already exists");
                return View();
            }

            await _blogService.EditAsync(dbBlog, blogEdit);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

