using Library.Models.Category;

namespace Library.Services.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewNodel>> GetAll();
    }
}
