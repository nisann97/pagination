using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using test.ViewModels.Blogs;
using test.Data;
using test.Models;
using test.ViewModels.Categories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace test.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }
      


            [HttpGet]
            public async Task<IActionResult> Index()
            {
                List<Blog> blogs = await _context.Blogs.OrderByDescending(m => m.Id).ToListAsync();
                return View(blogs);
            }

            [HttpGet]
            public async Task<IActionResult> Create()
            {

                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]

            public async Task<IActionResult> Create(BlogCreateVM blog)
            {

                if (!ModelState.IsValid)
                {
                    return View();
                }
            await _context.Blogs.AddAsync(new Blog { Title = blog.Title, Description = blog.Description, Date = blog.Date }) ;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

        //[HttpGet]
        //public async Task<IActionResult> Detail(int? id)
        //{
        //    if (id is null) return BadRequest();


        //Blog blog = await _context.Blogs.Where(m => m.Id == id).FirstOrDefaultAsync();
        //    if (blog is null) return NotFound();

        //    //CategoryDetailVM model = new()
        //    //{
        //    //    Name = Blog.Name,
        //    //    ProductCount = category.Products.Count()

        //    //};

        //    return View(model);
        //}



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();


            Blog blog = await _context.Blogs.Where(m => m.Id == id)
                                                       .FirstOrDefaultAsync();

            if (blog is null) return NotFound();

            _context.Blogs.Remove(blog);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}

