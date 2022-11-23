using HouseRentingSystem.Core.Contracts;
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
    public class AgentService : IAgentService
    {
        private readonly IRepository repo;

        public AgentService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task Create(string userId, string phoneNumber)
        {
            var agent = new Agent { UserId = userId, PhoneNumber = phoneNumber };
            await this.repo.AddAsync(agent);
            await this.repo.SaveChangesAsync();
        }

        public async Task<bool> ExistsById(string userId)
        {
            return await this.repo.AllReadonly<Agent>().AnyAsync(a => a.UserId == userId);
        }

        public async Task<bool> UserHasRents(string userId)
        {
            return await repo.AllReadonly<House>().AnyAsync(h => h.RenterId == userId);
        }

        public async Task<bool> UserWithPhoneNumberExists(string phoneNumber)
        {
            return await this.repo.AllReadonly<Agent>().AnyAsync(a => a.PhoneNumber == phoneNumber);    
        }

        public async Task<int> GetAgentId(string userId)
        {
            var agent = await this.repo.AllReadonly<Agent>().FirstOrDefaultAsync(a => a.UserId == userId);
            if (agent == null)
            {
                return 0;
            }

            return agent.Id;
        }
    }
}
