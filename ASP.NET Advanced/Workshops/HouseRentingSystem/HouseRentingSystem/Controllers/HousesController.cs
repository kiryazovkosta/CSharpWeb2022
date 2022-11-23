namespace HouseRentingSystem.Controllers
{
    using HouseRentingSystem.Core.Contracts;
    using HouseRentingSystem.Core.Models.House;
    using HouseRentingSystem.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class HousesController : Controller
    {
        private readonly IHouseService houseService;
        private readonly ICategoryService categoryService;
        private readonly IAgentService agentService;

        public HousesController(
            IHouseService houseService,
            ICategoryService categoryService,
            IAgentService agentService)
        {
            this.houseService = houseService;
            this.categoryService = categoryService;
            this.agentService = agentService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllHousesQueryModel query)
        {
            var queryResult = await this.houseService.All(
                query.Category,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllHousesQueryModel.HousesPerPage);

            query.TotalHousesCount = queryResult.TotalHousesCount;
            query.Houses = queryResult.Houses;

            var categories = await this.categoryService.AllCategoriesNames();
            query.Categories = categories;

            return View(query);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (!await this.houseService.Exists(id))
            {
                return BadRequest();
            }

            var model = await this.houseService.HouseDetailsById(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            IEnumerable<HouseServiceModel> myHouses = null!;

            var userId = this.User.Id();
            if (await this.agentService.ExistsById(userId))
            {
                var currentAgentId = await this.agentService.GetAgentId(userId);
                myHouses = await this.houseService.AllHousesByAgentId(currentAgentId);
            }
            else
            {
                myHouses = await this.houseService.AllHousesByUserId(userId);
            }

            return View(myHouses);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (!await this.agentService.ExistsById(this.User.Id()))
            {
                return RedirectToAction("Become", "Agents");
            }

            var model = new HouseModel()
            {
                Categories = await this.categoryService.GetAll(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(HouseModel model)
        {
            if (!await this.agentService.ExistsById(this.User.Id()))
            {
                return RedirectToAction("Become", "Agents");
            }

            if (!await this.categoryService.IsExists(model.CategoryId))
            {
                this.ModelState.AddModelError(nameof(model.CategoryId), "Category does not exists.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await this.categoryService.GetAll();
                return View(model);
            }

            var agentId = await this.agentService.GetAgentId(this.User.Id());
            var newHouseId = await this.houseService.CreateAsync(model, agentId);
            return RedirectToAction(nameof(Details), new { id = newHouseId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await this.houseService.Exists(id))
            {
                return BadRequest();
            }

            if (!await this.houseService.HasAgentWithId(id, this.User.Id()))
            {
                return Unauthorized();
            }

            var house = await this.houseService.HouseDetailsById(id);
            var categoryId = await this.houseService.GetHouseCategoryId(house.Id);

            var model = new HouseModel()
            {
                Title = house.Title,
                Address = house.Address,
                Description = house.Description,
                ImageUrl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                CategoryId = categoryId,
                Categories = await this.categoryService.GetAll(),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, HouseModel model)
        {
            if (!await this.houseService.Exists(id))
            {
                return BadRequest();
            }

            if (!await this.houseService.HasAgentWithId(id, this.User.Id()))
            {
                return Unauthorized();
            }

            if (!await this.categoryService.IsExists(model.CategoryId))
            {
                this.ModelState.AddModelError("CategoryId", "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await this.categoryService.GetAll();
                return View(model);
            }

            await this.houseService.Edit(id, model.Title, model.Address, model.Description, model.ImageUrl, model.PricePerMonth, model.CategoryId);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await this.houseService.Exists(id))
            {
                return BadRequest();
            }

            if (!await this.houseService.HasAgentWithId(id, this.User.Id()))
            {
                return Unauthorized();
            }

            var house = await this.houseService.HouseDetailsById(id);
            var model = new HouseDetailsViewModel()
            {
                Id = house.Id,
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HouseDetailsViewModel model)
        {
            if (!await this.houseService.Exists(model.Id))
            {
                return BadRequest();
            }

            if (!await this.houseService.HasAgentWithId(model.Id, this.User.Id()))
            {
                return Unauthorized();
            }

            await this.houseService.Delete(model.Id);
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Rent(int id)
        {
            if (!await this.houseService.Exists(id))
            {
                return BadRequest();
            }

            if (await agentService.ExistsById(this.User.Id()))
            {
                return Unauthorized();
            }

            if (await this.houseService.IsRented(id))
            {
                return BadRequest();
            }

            await this.houseService.Rent(id, this.User.Id());
            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            if (!await this.houseService.Exists(id) || 
                !await this.houseService.IsRented(id))
            {
                return BadRequest();
            }

            if (!await this.houseService.IsRentedByUserWithId(id, this.User.Id()))
            {
                return Unauthorized();
            }

            await this.houseService.Leave(id);
            return RedirectToAction(nameof(Mine));
        }


    }
}
