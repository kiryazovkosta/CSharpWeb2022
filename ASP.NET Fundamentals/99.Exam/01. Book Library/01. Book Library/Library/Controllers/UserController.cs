namespace Library.Controllers
{
    using Library.Data.Models;
    using Library.Models.User;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using static Library.Common.GlobalData.ControllersActionsNames;

    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserController(
            UserManager<ApplicationUser> userManagerParam,
            SignInManager<ApplicationUser> signInManagerParam)
        {
            this.userManager = userManagerParam;
            this.signInManager = signInManagerParam;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction(AllAction, BooksControllerName);
            }

            var model = new RegisterUserInputModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
            var result = await this.userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction(AllAction, BooksControllerName);
            }

            LoginUserViewModel model = new();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return View(model);
            }

            var result = await this.signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction(AllAction, BooksControllerName);
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(IndexAction, HomeControllerName);
        }
    }
}
