using HouseRentingSystem.Core.Models.House;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Core.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<HouseCategoryServiceModel>> GetAll();

        Task<bool> IsExists(int categoryId);

        Task<IEnumerable<string>> AllCategoriesNames();
    }
}
