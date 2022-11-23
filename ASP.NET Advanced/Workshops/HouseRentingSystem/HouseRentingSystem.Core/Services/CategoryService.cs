namespace HouseRentingSystem.Core.Services
{
    using HouseRentingSystem.Core.Contracts;
    using HouseRentingSystem.Core.Models.House;
    using HouseRentingSystem.Infrastructure.Data.Entities;
    using HouseRentingSystem.Infrastructure.Repo;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly IRepository repo;

        public CategoryService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<HouseCategoryServiceModel>> GetAll()
        {
            return await this.repo
                .AllReadonly<Category>()
                .Select(c => new HouseCategoryServiceModel { Id = c.Id, Name = c.Name, })
                .ToListAsync();
        }

        public async Task<bool> IsExists(int categoryId) 
        { 
            return await this.repo
                .AllReadonly<Category>()
                .AnyAsync(c => c.Id == categoryId);   
        }

        public async Task<IEnumerable<string>> AllCategoriesNames()
        {
            return await this.repo
                .All<Category>()
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();
        }
    }
}