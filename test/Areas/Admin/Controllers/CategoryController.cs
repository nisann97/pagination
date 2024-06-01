using System;
using System.Collections.Generic;
using test.Data;
using System.Linq;
using System.Threading.Tasks;
using test.Models;
using Microsoft.AspNetCore.Mvc;
using test.ViewModels.Categories;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace test.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {

            List<Category> categories = await _context.Categories.OrderByDescending(m => m.Id).ToListAsync();
            List<CategoryVM> model = categories.Select(m => new CategoryVM { Id = m.Id, Name = m.Name}).ToList(); 
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CategoryCreateVM category)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            bool existCategory = await _context.Categories.AnyAsync(m => m.Name == category.Name);
            if (existCategory)
            {
                ModelState.AddModelError("Name", "This category already exists");
                return View();
            }
            await _context.Categories.AddAsync(new Category { Name = category.Name});
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();


            Category category = await _context.Categories.Where(m => m.Id == id)
                                                          .Include(m => m.Products)
                                                          .FirstOrDefaultAsync();

            if (category is null) return NotFound();

            CategoryDetailVM model = new()
            {
                Name = category.Name,
                ProductCount = category.Products.Count()

            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();


            Category category = await _context.Categories.Where(m => m.Id == id)
                                                          .Include(m => m.Products)
                                                          .FirstOrDefaultAsync();

            if (category is null) return NotFound();

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();


            Category category = await _context.Categories.Where(m => m.Id == id)
                                                          .FirstOrDefaultAsync();

            if (category is null) return NotFound();


            return View(new CategoryEditVM { Id = category.Id, Name = category.Name});

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CategoryEditVM category)
        {
            if (id is null) return BadRequest();


            Category existCategory = await _context.Categories.Where(m => m.Id == id)
                                                          .FirstOrDefaultAsync();

            if (existCategory is null) return NotFound();


            bool existingCategory = await _context.Categories.AnyAsync(m => m.Name == category.Name);

            if (existingCategory)
            {
                ModelState.AddModelError("Name", "This category already exists");
                return View();
            }

            existCategory.Name = category.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}

