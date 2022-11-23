using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Core.Models.House
{
    using static HouseRentingSystem.Infrastructure.Common.DataConstants.House;

    public class HouseModel
    {
        [Required]
        [StringLength(HouseMaxTitle, MinimumLength = HouseMinTitle)]
        public string Title { get; init; } = null!;

        [Required]
        [StringLength(HouseMaxAddress, MinimumLength = HouseMinAddress)]
        public string Address { get; init; } = null!;

        [Required]
        [StringLength(HouseMaxDescription, MinimumLength = HouseMinDescription)]
        public string Description { get; init; } = null!;

        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; init; } = null!;

        [Required]
        [Range(HouseMinPricePerMonth, HouseMaxPricePerMonth, ErrorMessage = "Price Per Month must be a positive number and less than {2}")]
        [Display(Name = "Price Per Month")]
        public decimal PricePerMonth { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<HouseCategoryServiceModel> Categories { get; set; }
            = new HashSet<HouseCategoryServiceModel>();
    }
}
