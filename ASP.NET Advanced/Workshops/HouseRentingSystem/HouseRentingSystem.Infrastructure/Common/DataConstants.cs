using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Infrastructure.Common
{
    public class DataConstants
    {
        public class Category
        {
            public const int CategoryMaxName = 50;
        }

        public class Agent
        {
            public const int AgentMaxPhoneNumber = 15;
            public const int AgentMinPhoneNumber = 7;
        }

        public class House
        {
            public const int HouseMaxTitle = 50;
            public const int HouseMinTitle = 10;

            public const int HouseMaxAddress = 150;
            public const int HouseMinAddress = 30;

            public const int HouseMaxDescription = 500;
            public const int HouseMinDescription = 50;

            public const double HouseMaxPricePerMonth = 2000.00;
            public const double HouseMinPricePerMonth = 0.00;
        }
    }
}
