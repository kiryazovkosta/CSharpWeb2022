using System.ComponentModel;

namespace HouseRentingSystem.Core.Models.House
{
    public class HouseServiceModel
    {
        public int Id { get; init; }

        public string Title { get; init; } = null!;

        public string Address { get; init; } = null!;

        [DisplayName("Image URL")]
        public string ImageUrl { get; init; } = null!;

        [DisplayName("Price Per Month")]
        public decimal PricePerMonth { get; init; }

        [DisplayName("IS Rented")]
        public bool IsRented { get; init; }
    }
}
