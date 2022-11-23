using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.Statistics;
using HouseRentingSystem.Infrastructure.Data;
using HouseRentingSystem.Infrastructure.Data.Entities;
using HouseRentingSystem.Infrastructure.Repo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Core.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IRepository repo;

        public StatisticsService(IRepository repository)
        {
            this.repo = repository;
        }

        public async Task<StatisticsServiceModel> Total()
        {
            return new StatisticsServiceModel
            {
                TotalHouses = await this.repo.All<House>().CountAsync(),
                TotalRents = await this.repo.All<House>().Where(h => h.RenterId != null).CountAsync()
            };
        }
    }
}
