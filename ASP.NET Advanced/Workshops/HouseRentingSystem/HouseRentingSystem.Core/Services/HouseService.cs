using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Enumerations;
using HouseRentingSystem.Core.Models.Agent;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Infrastructure.Data.Entities;
using HouseRentingSystem.Infrastructure.Repo;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Core.Services
{
    public class HouseService : IHouseService
    {
        private readonly IRepository repo;

        public HouseService(IRepository repository)
        {
            this.repo = repository;
        }

        public async Task<IEnumerable<HouseHomeModel>> LastThreeHouses()
        {
            return await repo.AllReadonly<House>()
                .OrderByDescending(h => h.Id)
                .Take(3)
                .Select(h => new HouseHomeModel
                {
                    Id = h.Id,
                    Title = h.Title,
                    ImageUrl = h.ImageUrl
                })
                .ToListAsync();
        }

        public async Task<int> CreateAsync(HouseModel model, int agentId)
        {
            var house = new House()
            {
                Title = model.Title,
                Address = model.Address,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerMonth = model.PricePerMonth,
                CategoryId = model.CategoryId,
                AgentId = agentId
            };

            await this.repo.AddAsync<House>(house);
            await this.repo.SaveChangesAsync();

            return house.Id;
        }

        public async Task<HouseQueryServiceModel> All(
            string? category = null,
            string? searchTerm = null,
            HouseSorting sorting = HouseSorting.Newest,
            int currentPage = 1,
            int housesPerPage = 1)
        {
            var houseQuery = this.repo.All<House>();

            if (!string.IsNullOrWhiteSpace(category))
            {
                houseQuery = houseQuery.Include(h => h.Category).Where(h => h.Category.Name == category);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                houseQuery = houseQuery.Where(h =>
                    h.Title.ToLower().Contains(searchTerm.ToLower()) ||
                    h.Address.ToLower().Contains(searchTerm.ToLower()) ||
                    h.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            switch (sorting)
            {
                case HouseSorting.Price:
                    houseQuery = houseQuery.OrderBy(h => h.PricePerMonth);
                    break;
                case HouseSorting.NotRentedFirst:
                    houseQuery = houseQuery.OrderBy(h => h.RenterId != null).ThenByDescending(h => h.Id);
                    break;
                default:
                    houseQuery = houseQuery.OrderByDescending(h => h.Id);
                    break;
            }

            var houses = await houseQuery
                .Skip((currentPage - 1) * housesPerPage)
                .Take(housesPerPage)
                .Select(h => new HouseServiceModel
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth
                })
                .ToListAsync();
            return new HouseQueryServiceModel { TotalHousesCount = houses.Count, Houses = houses };
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByAgentId(int agentId)
        {
            return await this.repo
                .All<House>()
                .Where(h => h.AgentId == agentId)
                .Select(h => new HouseServiceModel
                {
                    Id = h.Id,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth,
                    Title = h.Title
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByUserId(string userId)
        {
            return await this.repo
                .All<House>()
                .Where(h => h.RenterId == userId)
                .Select(h => new HouseServiceModel
                {
                    Id = h.Id,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth,
                    Title = h.Title
                })
                .ToListAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await this.repo
                .AllReadonly<House>()
                .AnyAsync(h => h.Id == id);
        }

        public async Task<HouseDetailsServiceModel> HouseDetailsById(int id)
        {
            return await this.repo
                .All<House>()
                .Include(h => h.Category)
                .Include(h => h.Agent)
                .ThenInclude(a => a.User)
                .Where(h => h.Id == id)
                .Select(h => new HouseDetailsServiceModel()
                {
                    Id = h.Id, 
                    Title = h.Title, 
                    Address = h.Address, 
                    Description = h.Description, 
                    IsRented = h.RenterId != null, 
                    ImageUrl = h.ImageUrl, 
                    PricePerMonth = h.PricePerMonth, 
                    Category = h.Category.Name,
                    Agent = new AgentServiceModel()
                    {
                        PhoneNumber = h.Agent.PhoneNumber,
                        Email = h.Agent.User.Email
                    }
                })
                .FirstOrDefaultAsync() ?? new HouseDetailsServiceModel();

        }

        public async Task Edit(int houseId, string title, string address, string description, 
            string imageUrl, decimal price, int categoryId)
        {
            var house = await this.repo.GetByIdAsync<House>(houseId);
            if (house != null)
            {
                house.Title = title;
                house.Address = address;
                house.Description = description;
                house.ImageUrl = imageUrl;
                house.PricePerMonth = price;
                house.CategoryId = categoryId;

                await this.repo.SaveChangesAsync();
            }
        }

        public async Task<bool> HasAgentWithId(int houseId, string currentUserId)
        {
            var house = await this.repo.GetByIdAsync<House>(houseId);
            var agent = await this.repo.All<Agent>().FirstOrDefaultAsync(a => a.Id == house.AgentId);

            if (house == null || 
                agent == null ||
                agent.UserId != currentUserId)
            {
                return false;
            }

            return true; 
        }

        public async Task<int> GetHouseCategoryId(int houseId)
        {
            return (await this.repo.All<House>().FirstAsync(h => h.Id == houseId)).CategoryId;
        }

        public async Task Delete(int houseId)
        {
            await this.repo.DeleteAsync<House>(houseId);
            await this.repo.SaveChangesAsync();
        }

        public async Task<bool> IsRented(int id)
        {
            return (await this.repo.GetByIdAsync<House>(id)).RenterId != null;
        }

        public async Task<bool> IsRentedByUserWithId(int houseId, string userId)
        {
            var house = await this.repo.GetByIdAsync<House>(houseId);
            if (house == null)
            {
                return false;
            }
            if (house.RenterId != userId)
            {
                return false;
            }

            return true;
        }

        public async Task Rent(int houseId, string userId)
        {
            var house = await this.repo.GetByIdAsync<House>(houseId);
            house.RenterId = userId;
            await this.repo.SaveChangesAsync();
        }

        public async Task Leave(int houseId)
        {
            var house = await this.repo.GetByIdAsync<House>(houseId);
            house.RenterId = null;
            await this.repo.SaveChangesAsync();
        }
    }
}
