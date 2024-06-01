using System;
using test.Models;
using test.ViewModels.Blogs;
namespace test.Services.Interfaces
{
	public interface IBlogService
	{
		Task<List<BlogVM>> GetAllOrderByDescAsync();

		Task CreateAsync(BlogCreateVM blog);

		Task<bool> ExistAsync(string title, string desc);

		Task<bool> ExistForTitleAsync(string title);

		Task<Blog> GetByIdAsync(int id);

		Task DeleteAsync(Blog blog);

		Task EditAsync(Blog blog, BlogEditVM blogEdit);


	}
}

