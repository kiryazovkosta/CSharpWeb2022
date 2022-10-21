using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watchlist.ViewModels;

namespace Watchlist.Services.Contracts
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreViewModel>> GetAll();
    }
}
