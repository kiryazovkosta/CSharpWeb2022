using Library.Data;
using Library.Models.Category;
using Library.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly LibraryDbContext data;

        public CategoryService(LibraryDbContext libraryDbContext)
        {
            this.data = libraryDbContext ?? throw new ArgumentNullException(nameof(libraryDbContext));
        }
        public async Task<IEnumerable<CategoryViewNodel>> GetAll()
        {
            var categories = await this.data.Categories
                .Select(c => new CategoryViewNodel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
            return categories;
        }
    }
}
