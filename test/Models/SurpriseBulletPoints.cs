using System;
namespace test.Models
{
	public class SurpriseBulletPoints : BaseEntity
	{
		public string Video { get; set; }
		public string Description { get; set; }
		public int SurpriseId { get; set; }
		public Surprise Surprise { get; set; }
	}
}

