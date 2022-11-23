namespace HouseRentingSystem.Controllers
{
    using HouseRentingSystem.Core.Contracts;
    using HouseRentingSystem.Core.Models.Agent;
    using HouseRentingSystem.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class AgentsController : Controller
    {
        private readonly IAgentService agentService;

        public AgentsController(IAgentService agentService)
        {
            this.agentService = agentService;
        }

        [HttpGet]
        public async Task<IActionResult> Become()
        {
            if (await this.agentService.ExistsById(this.User.Id()))
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Become(BecomeAgentModel model)
        {
            var userId = this.User.Id();
            if (await this.agentService.ExistsById(userId))
            {
                return BadRequest();
            }

            if (await this.agentService.UserWithPhoneNumberExists(model.PhoneNumber))
            {
                ModelState.AddModelError(nameof(model.PhoneNumber), "Phone number already exists. Enter another one.");
            }

            if (await this.agentService.UserHasRents(userId))
            {
                ModelState.AddModelError("Error", "You should have no rents to become an agent!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.agentService.Create(userId, model.PhoneNumber);

            return RedirectToAction("All", "Houses");
        }
    }
}
