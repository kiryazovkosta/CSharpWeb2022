using Watchlist.ViewModels;

namespace Watchlist.Services.Contracts
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieViewModel>> GetAll();

        Task Add(MovieInputModel model);

        Task<IEnumerable<MovieViewModel>> GetAllWatched(string userId);

        Task AddToCollection(MovieUserModel model);

        Task RemoveFromCollection(MovieUserModel model);
    }
}
