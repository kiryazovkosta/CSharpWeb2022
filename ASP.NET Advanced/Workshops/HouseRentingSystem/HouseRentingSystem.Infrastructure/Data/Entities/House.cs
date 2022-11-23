namespace HouseRentingSystem.Infrastructure.Data.Entities
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static HouseRentingSystem.Infrastructure.Common.DataConstants.House;
    public class House
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(HouseMaxTitle)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(HouseMaxAddress)]
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(HouseMaxDescription)]
        public string Description { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        [Precision(15, 2)]
        public decimal PricePerMonth { get; set; }

        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Agent))]
        public int AgentId { get; set; }
        public Agent Agent { get; set; } = null!;

        [ForeignKey(nameof(Renter))]
        public string? RenterId { get; set; }
        public IdentityUser? Renter { get; set; }
    }
}

