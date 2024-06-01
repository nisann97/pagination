using System;
namespace test.Models
{
	public class Surprise : BaseEntity
	{
		public string Video { get; set; }
		public string Title { get; set; }
		public string Paragraph { get; set; }
		public ICollection<SurpriseBulletPoints> BulletPoints { get; set; }
	}
}

