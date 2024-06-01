using System;
namespace test.ViewModels.Carts
{
	public class CartVM
	{
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public int Count { get; set; }
    }

}

