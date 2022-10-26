namespace Library.Controllers
{
    using Library.Models;
    using Library.Models.ApplicationUserBook;
    using Library.Models.Book;
    using Library.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBookService bookService;
        private readonly ICategoryService categoryService;

        public BooksController(
            IBookService bookServiceParam, 
            ICategoryService  categoryServiceParam)
        {
            this.bookService = bookServiceParam ?? throw new ArgumentNullException(nameof(bookService));
            this.categoryService = categoryServiceParam ?? throw new ArgumentNullException(nameof(categoryService)); 
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var books = await this.bookService.GetAll();
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var books = await this.bookService.GetMine(userId);
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new BookInputModel
            {
                Categories = await this.categoryService.GetAll()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BookInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await this.bookService.Add(model);
                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                this.ModelState.AddModelError("", "Something went wrong");
                return this.View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCollection(int bookId)
        {
            var model = new UserBookInputModel
            {
                UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
                BookId = bookId
            };

            await this.bookService.AddToCollection(model);
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int bookId)
        {
            var model = new UserBookInputModel
            {
                UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
                BookId = bookId
            };

            await this.bookService.RemoveFromCollection(model);
            return RedirectToAction(nameof(Mine));
        }

    }
}
