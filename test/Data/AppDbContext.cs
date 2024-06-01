using System;
using test.Models;
using Microsoft.EntityFrameworkCore;
namespace test.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
		{

		}

		public DbSet<Slider> Sliders { get; set; }
		public DbSet<SliderInfo> SlidersInfo { get; set; }
		public DbSet<Category> Categories  { get; set; }
		public DbSet<Product> Products  { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Surprise> Surprises { get; set; }
        public DbSet<SurpriseBulletPoints> SurpriseBulletPoints { get; set; }
		public DbSet<ExpertPanel> ExpertPanel { get; set; }
		public DbSet<Expert> Experts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Setting> Settings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .HasQueryFilter(m => !m.SoftDeleted);


            modelBuilder.Entity<Blog>()
                .HasData(
                new Blog
                {
                    Id = 1,
                    Title = "Blog title1",
                    Description = "Desc1",
                    Date = DateTime.Now,
                    Image = "blog-feature-img-1.jpg"


                },


                    new Blog
                    {
                        Id = 2,
                        Title = "Blog title2",
                        Description = "Desc2",
                        Date = DateTime.Now,
                        Image = "blog-feature-img-3.jpg"


                    },
                     new Blog
                     {
                         Id = 3,
                         Title = "Blog title3",
                         Description = "Desc3",
                         Date = DateTime.Now,
                         Image = "blog-feature-img-4.jpg"


                     }

                );
        }
    }


}

