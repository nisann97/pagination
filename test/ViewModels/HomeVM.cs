using System;
using test.Models;
namespace test.ViewModels
{
	public class HomeVM
	{
		public List<Category> Categories { get; set; }
		public List<Product> Products  { get; set; }
		public Surprise Surprise { get; set; }
		public List<SurpriseBulletPoints> SurpriseBulletPoints { get; set; }
		public ExpertPanel ExpertPanel { get; set; }
		public List<Expert> Experts { get; set; }
		public List<Blog> Blogs { get; set; }
	}
}

