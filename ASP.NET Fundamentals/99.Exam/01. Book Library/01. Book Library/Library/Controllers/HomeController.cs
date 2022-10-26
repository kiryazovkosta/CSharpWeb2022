using Microsoft.AspNetCore.Mvc;
using static Library.Common.GlobalData.ControllersActionsNames;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction(AllAction, BooksControllerName);
            }

            return View();
        }
    }
}