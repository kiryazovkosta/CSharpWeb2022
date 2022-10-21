using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watchlist.Data;
using Watchlist.Models;
using Watchlist.Services.Contracts;
using Watchlist.ViewModels;

namespace Watchlist.Services
{
    public class MovieService : IMovieService
    {
        private readonly WatchlistDbContext context;

        public MovieService(WatchlistDbContext watchlistDbContext)
        {
            this.context = watchlistDbContext;
        }

        public async Task Add(MovieInputModel model)
        {
            var movie = new Movie { Title = model.Title, Director = model.Director, ImageUrl = model.ImageUrl, Rating = model.Rating, GenreId = model.GenreId };
            await this.context.Movies.AddAsync(movie);
            await this.context.SaveChangesAsync();
        }

        public async Task AddToCollection(MovieUserModel model)
        {
            var movie = await this.context.Movies.FirstOrDefaultAsync(m => m.Id == model.MovieId);
            if (movie is null)
            {
                throw new ArgumentException(nameof(model.MovieId));
            }

            var user = await this.context.Users.Include(u => u.UserMovies).FirstOrDefaultAsync(u => u.Id == model.UserId);
            if (user is null)
            {
                throw new ArgumentException(nameof(model.UserId));
            }

            if (!user.UserMovies.Any(um => um.MovieId == model.MovieId))
            {
                user.UserMovies.Add(new UserMovie { UserId = model.UserId, MovieId = model.MovieId });
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MovieViewModel>> GetAll()
        {
            var movies = await this.context.Movies
                .Include(m => m.Genre)
                .Select(m => new MovieViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Director = m.Director,
                    ImageUrl = m.ImageUrl,
                    Rating = m.Rating,
                    Genre = m.Genre.Name
                })
                .ToListAsync();
            return movies;
        }

        public async Task<IEnumerable<MovieViewModel>> GetAllWatched(string userId)
        {
            var user = await this.context.Users.Include(u => u.UserMovies).ThenInclude(um => um.Movie).ThenInclude(m => m.Genre).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new ArgumentException(nameof(user));
            }

            var movies = user.UserMovies.Select(um => new MovieViewModel { Id = um.MovieId, ImageUrl = um.Movie.ImageUrl }).ToList();

            return movies;
        }

        public async Task RemoveFromCollection(MovieUserModel model)
        {
            var movie = await this.context.Movies.FirstOrDefaultAsync(m => m.Id == model.MovieId);
            if (movie is null)
            {
                throw new ArgumentException(nameof(model.MovieId));
            }

            var user = await this.context.Users.Include(u => u.UserMovies).FirstOrDefaultAsync(u => u.Id == model.UserId);
            if (user is null)
            {
                throw new ArgumentException(nameof(model.UserId));
            }

            var userMovie = user.UserMovies.FirstOrDefault(um => um.MovieId == model.MovieId);
            if (userMovie is not null)
            {
                user.UserMovies.Remove(userMovie);
                await this.context.SaveChangesAsync();
            }
        }
    }
}
