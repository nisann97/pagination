﻿using System;
using test.Models;

namespace test.ViewModels.Baskets
{
	public class BasketProductsVM
	{

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public int Count { get; set; }
        
    }
}

