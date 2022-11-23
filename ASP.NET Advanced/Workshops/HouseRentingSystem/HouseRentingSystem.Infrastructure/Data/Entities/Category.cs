namespace HouseRentingSystem.Infrastructure.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using static HouseRentingSystem.Infrastructure.Common.DataConstants.Category;

    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(CategoryMaxName)]
        public string Name { get; set; } = null!;

        public ICollection<House> Houses { get; set; }
            = new HashSet<House>();
    }
}