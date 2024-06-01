using System;
using Microsoft.AspNetCore.Mvc;
using test.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using test.ViewModels;
using Newtonsoft.Json;
using test.ViewModels.Baskets;

namespace test.ViewComponents
{
	public class HeaderViewComponent : ViewComponent
	{

        private readonly ISettingService _settingService;
        private readonly IHttpContextAccessor _accessor;

        public HeaderViewComponent(ISettingService settingService,
                                    IHttpContextAccessor accessor)
        {
            _settingService = settingService;
            _accessor = accessor;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            Dictionary<string, string> settingDatas = await _settingService.GetAllAsync();

            List<BasketVM> basketProducts = new();

            if (_accessor.HttpContext.Request.Cookies["basket"] is not null)
            {
                basketProducts = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);

            }

            HeaderVM response = new()
            {
                Settings = settingDatas,
                BasketCount = basketProducts.Sum(m=>m.Count),
                BasketTotalPrice = basketProducts.Sum(m=> m.Count * m.Price)


            };
            return await Task.FromResult(View(response));

        }
    }
}

