using HouseRentingSystem.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Core.Models.House
{
    public class AllHousesQueryModel
    {
        public const int HousesPerPage = 3;

        [Display(Name = "Search by category")]
        public string Category { get; init; } = null!;

        public string SearchTerm { get; init; } = null!;

        public HouseSorting Sorting { get; init; }

        public int CurrentPage { get; set; } = 1;

        public int TotalHousesCount { get; set; }

        public IEnumerable<string> Categories { get; set; } = null!;

        public IEnumerable<HouseServiceModel> Houses { get; set; }
            = new HashSet<HouseServiceModel>();
    }
}
