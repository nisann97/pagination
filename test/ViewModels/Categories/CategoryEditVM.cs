using System;
using System.ComponentModel.DataAnnotations;

namespace test.ViewModels.Categories
{
	public class CategoryEditVM
	{
        [Required(ErrorMessage = "This input cannot be empty")]
        [StringLength(20, ErrorMessage = "Cannot be more than 20")]
        public int Id { get; set; }
		public string Name { get; set; }
	}
}

