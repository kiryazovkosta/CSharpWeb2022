namespace HouseRentingSystem.Core.Models.Agent
{
    using System.ComponentModel.DataAnnotations;
    using static HouseRentingSystem.Infrastructure.Common.DataConstants.Agent;
    public class BecomeAgentModel
    {
        [Required]
        [StringLength(AgentMaxPhoneNumber, MinimumLength = AgentMinPhoneNumber)]
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; init; } = null!;
    }
}