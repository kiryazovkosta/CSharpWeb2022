using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watchlist.Services.Contracts;
using Watchlist.ViewModels;

namespace Watchlist.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IMovieService movieService;
        private readonly IGenreService genreService;

        public MoviesController(
            IMovieService movieService, 
            IGenreService genreService)
        {
            this.movieService = movieService;
            this.genreService = genreService;
        }

        public async Task<IActionResult> All()
        {
            var movies = await movieService.GetAll();
            return View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new MovieInputModel()
            {
                Genres = await this.genreService.GetAll()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MovieInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await this.movieService.Add(model);
                return RedirectToAction("All", "Movies");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Watched()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var model = await movieService.GetAllWatched(userId);
            return this.View(model);
        }



        [HttpPost]
        public async Task<IActionResult> AddToCollection(int movieId)
        {
            var model = new MovieUserModel
            {
                UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
                MovieId = movieId
            };

            await movieService.AddToCollection(model);
            return RedirectToAction("All", "Movies");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCollection(int movieId)
        {
            var model = new MovieUserModel
            {
                UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
                MovieId = movieId
            };

            await movieService.RemoveFromCollection(model);
            return RedirectToAction("Watched", "Movies");
        }
    }
}
