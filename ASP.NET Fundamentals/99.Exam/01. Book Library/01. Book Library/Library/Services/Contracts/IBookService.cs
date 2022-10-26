namespace Library.Services.Contracts
{
    using Library.Models.ApplicationUserBook;
    using Library.Models.Book;

    public interface IBookService
    {
        Task<IEnumerable<BookViewModel>> GetAll();

        Task<IEnumerable<BookViewModel>> GetMine(string userId);

        Task Add(BookInputModel model);

        Task AddToCollection(UserBookInputModel model);

        Task RemoveFromCollection(UserBookInputModel model);
    }
}