using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watchlist.Data;
using Watchlist.Services.Contracts;
using Watchlist.ViewModels;

namespace Watchlist.Services
{
    public class GenreService : IGenreService
    {
        private readonly WatchlistDbContext context;

        public GenreService(WatchlistDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<GenreViewModel>> GetAll()
        {
            var genres = await this.context.Genres
                .Select(g => new GenreViewModel
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToListAsync();
            return genres;
        }
    }
}
