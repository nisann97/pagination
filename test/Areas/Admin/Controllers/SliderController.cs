﻿using System;
using System.Collections.Generic;
using System.Linq;
using test.ViewModels.Sliders;
using System.Threading.Tasks;
using test.Helpers.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;
using test.ViewModels.Categories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace test.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context,
                                IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.ToListAsync();

            List<SliderVM> result = sliders.Select(m => new SliderVM { Id = m.Id, Image = m.Image }).ToList();
            return View(model: result);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM request)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            foreach(var item in request.Images)
            {
                if (item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Images", "File must be only in image format");
                    return View();
                }

                if (!item.CheckFileSize(200))
                {
                    ModelState.AddModelError("Images", "Image size cannot be more than 200");
                    return View();
                }
            }

            foreach(var item in request.Images)
            {
                string fileName = Guid.NewGuid().ToString() + " -" + item.FileName;

                string path = Path.Combine(_env.WebRootPath, "img", fileName);

                await item.SaveFileToLocalAsync(path);

                await _context.Sliders.AddAsync(new Slider { Image = fileName });

                await _context.SaveChangesAsync();

            }

         


          
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete (int? id)
        {
            if (id is null) return BadRequest();

            var slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if (slider is null) return NotFound();
            string path = Path.Combine(_env.WebRootPath, "img", slider.Image);

            path.DeleteFileFromLocal();

            _context.Sliders.Remove(slider);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            var slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if (slider is null) return NotFound();

            return View(new SliderEditVM { Image = slider.Image });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderEditVM request)
        {
            if (id is null) return BadRequest();

            var slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if (slider is null) return NotFound();

            if (request.NewImage is null) return RedirectToAction(nameof(Index));

            if (!request.NewImage.CheckFileType("image/"))
            {
                ModelState.AddModelError("NewImage", "File must be only in image format");
                request.Image = slider.Image;
                return View(request);
            }

            if (!request.NewImage.CheckFileSize(200))
            {
                ModelState.AddModelError("NewImage", "Size cannot be more than 200kb");
                request.Image = slider.Image;
                return View(request);
            }


            string oldPath = Path.Combine(_env.WebRootPath, "img", slider.Image);

            oldPath.DeleteFileFromLocal();

            string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
            string newPath = Path.Combine(_env.WebRootPath, "img", fileName);
            await request.NewImage.SaveFileToLocalAsync(newPath);

            slider.Image = fileName;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            var slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);

            if (slider is null) return NotFound();
            return View(new SliderDetailVM { Image = slider.Image});
        }


    }
    
}

