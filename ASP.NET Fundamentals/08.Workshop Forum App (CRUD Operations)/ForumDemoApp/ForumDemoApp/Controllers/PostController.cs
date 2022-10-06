using ForumDemoApp.Data;
using ForumDemoApp.Data.Models;
using ForumDemoApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumDemoApp.Controllers
{
    public class PostController : Controller
    {
        private readonly ForumAppDbContext context;



        public PostController(ForumAppDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await context.Posts
                .Select(p => new PostViewModel()
                { 
                    Id = p.Id, 
                    Title = p.Title, 
                    Content = p.Content 
                })
                .ToListAsync();
            return View(model); 
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new PostViewModel();

            ViewData["Title"] = "Add new Post";
            return View("Edit", model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = new PostViewModel();

            ViewData["Title"] = "Add new Post";
            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = model.Id == 0 ? "Add a Post" : "Edit Post";
                return View(model);
            }

            if (model.Id == 0)
            {
                context.Posts.Add(new Post() { Title = model.Title, Content = model.Content });
            }
            else
            {
                var post = await context.Posts.FindAsync(model.Id);
                if (post != null)
                {
                    post.Title = model.Title;
                    post.Content = model.Content;
                }
            }

            await context.SaveChangesAsync(); 
            
            return RedirectToAction(nameof(Index));
        }
    }
}
