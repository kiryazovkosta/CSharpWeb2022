using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Watchlist.Models;
using Watchlist.ViewModels;

namespace Watchlist.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public UserController(
            UserManager<User> userManagerParam,
            SignInManager<User> signInManagerParam)
        {
            this.userManager = userManagerParam;
            this.signInManager = signInManagerParam;
        }

        [HttpGet]
        public IActionResult Register()
        {
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

            var user = new User { UserName = model.UserName, Email = model.Email };
            var result = await this.userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login", "User");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
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
                return RedirectToAction("All", "Movies");
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionDescriptor.DisplayName != "Logout" && User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "All", action = "Index" }));    
            }

            base.OnActionExecuting(context);
        }
    }
}
