using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using test.Data;
using test.Models;
using test.ViewModels.Baskets;
using test.Services.Interfaces;
using test.ViewModels;

namespace test.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    private readonly IHttpContextAccessor _accessor;

    public HomeController(AppDbContext context,
                          IHttpContextAccessor accessor)
    {
        _context = context;
        _accessor = accessor;
    }


    public async Task<IActionResult>Index()
    {
        List<Category> categories = await _context.Categories.Include(m=>m.Products).Where(m => !m.SoftDeleted && m.Products.Count !=0).ToListAsync();
        List<Product> products = await _context.Products.Include(m => m.ProductImages).ToListAsync();
        Surprise surprise = await _context.Surprises.FirstOrDefaultAsync();
        List<SurpriseBulletPoints> surpriseBulletPoints = await  _context.SurpriseBulletPoints.ToListAsync();
        ExpertPanel expertPanel = await _context.ExpertPanel.FirstOrDefaultAsync();
        List<Expert> experts = await _context.Experts.ToListAsync();
        List<Blog> blogs = await _context.Blogs.Where(m=>!m.SoftDeleted).ToListAsync();


        //string name = "P418";

        //_accessor.HttpContext.Response.Cookies.Append("name", name);

        HomeVM model = new()
        {
         
            Categories = categories,
            Products = products,
            Surprise = surprise,
            SurpriseBulletPoints = surpriseBulletPoints,
            ExpertPanel = expertPanel,
            Experts = experts,
            Blogs = blogs

        };

        return View(model);
    } 

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBasket(int? id)
    {

        if (id is null) return BadRequest();
        //_accessor.HttpContext.Response.Cookies.Append("name", name);
        //_accessor.HttpContext.Request.Cookies["name"];

        List<BasketVM> basketProducts = null;

        if (_accessor.HttpContext.Request.Cookies["basket"] is not null)
        {
            basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);

            
        }
        else
        {
            basketProducts = new List<BasketVM>();
        }

        var dbProduct = await _context.Products.FirstOrDefaultAsync(m => m.Id == (int)id);

        var existProduct = basketProducts.FirstOrDefault(m => m.Id == (int)id);
        if (existProduct is not null)
        {
            existProduct.Count++; 
        }
        else
        {
            basketProducts.Add(new BasketVM
            {
                Id = (int)id,
                Count = 1,
                Price = dbProduct.Price
            });
        }

       

        _accessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketProducts));

        int count = basketProducts.Sum(m => m.Count);
        decimal total = basketProducts.Sum(m => m.Count * m.Price);

        return Ok(new { count, total });

    }

}



