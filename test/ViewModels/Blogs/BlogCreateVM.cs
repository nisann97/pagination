using System;
using System.ComponentModel.DataAnnotations;

namespace test.ViewModels.Blogs
{
	public class BlogCreateVM
	{
        [Required(ErrorMessage = "This input cannot be empty")]
        [StringLength(20, ErrorMessage = "Cannot be more than 20")]
        public string Title { get; set; }
        [Required(ErrorMessage = "This input cannot be empty")]
        [StringLength(50, ErrorMessage = "Cannot be more than 50")]
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Image { get; set; }
    }
}

