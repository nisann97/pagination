using System;
namespace test.Models
{
	public class Expert : BaseEntity
	{
        public string Image { get; set; }
        public string Name { get; set; }
		public string Profession { get; set; }
		public int ExpertPanelId{ get; set; }
		public ExpertPanel ExpertPanel { get; set; }
	}
}

