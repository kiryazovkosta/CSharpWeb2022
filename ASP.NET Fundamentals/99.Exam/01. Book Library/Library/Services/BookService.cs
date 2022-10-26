using Library.Data;
using Library.Data.Models;
using Library.Models.ApplicationUserBook;
using Library.Models.Book;
using Library.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext data;

        public BookService(LibraryDbContext libraryDbContext)
        {
            this.data = libraryDbContext;
        }

        public async Task<IEnumerable<BookViewModel>> GetAll()
        {
            var books = await this.data.Books
                .Include(b => b.Category)
                .Select(b => new BookViewModel 
                { 
                    Id = b.Id, 
                    Title = b.Title, 
                    Author = b.Author, 
                    ImageUrl = b.ImageUrl, 
                    Rating = b.Rating, 
                    Category = b.Category.Name
                })
                .ToListAsync();
            return books;
        }

        public async Task<IEnumerable<BookViewModel>> GetMine(string userId)
        {
            var user = await this.data.Users
                .Where(u => u.Id == userId)
                .Include(u => u.ApplicationUsersBooks)
                .ThenInclude(aub => aub.Book)
                .ThenInclude(b => b.Category)
                .FirstOrDefaultAsync();
            if (user is null)
            {
                throw new ArgumentException(nameof(user));
            }

            var books = user.ApplicationUsersBooks
                .Select(ub => new BookViewModel 
                { 
                    Id = ub.BookId, 
                    Title = ub.Book.Title, 
                    Author = ub.Book.Author,
                    Description = ub.Book.Description,
                    ImageUrl = ub.Book.ImageUrl,
                    Category = ub.Book.Category.Name
                })
                .ToList();

            return books;
        }

        public async Task Add(BookInputModel model)
        {
            var book = new Book
            {
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Rating = model.Rating,
                CategoryId = model.CategoryId,
            };
            await this.data.Books.AddAsync(book);
            await this.data.SaveChangesAsync();
        }

        public async Task AddToCollection(UserBookInputModel model)
        {
            var user = await this.data.Users
                .Include(u => u.ApplicationUsersBooks)
                .FirstOrDefaultAsync(u => u.Id == model.UserId);
            if (user is null)
            {
                throw new ArgumentException(nameof(model.UserId));
            }

            var movie = await this.data.Books
                .FirstOrDefaultAsync(b => b.Id == model.BookId);
            if (movie is null)
            {
                throw new ArgumentException(nameof(model.BookId));
            }

            if (!user.ApplicationUsersBooks.Any(um => um.BookId == model.BookId))
            {
                user.ApplicationUsersBooks.Add(
                    new ApplicationUserBook 
                    { 
                        ApplicationUserId = model.UserId, 
                        BookId = model.BookId 
                    });
                await this.data.SaveChangesAsync();
            }
        }

        public async Task RemoveFromCollection(UserBookInputModel model)
        {
            var user = await this.data.Users
                .Include(u => u.ApplicationUsersBooks)
                .FirstOrDefaultAsync(u => u.Id == model.UserId);
            if (user is null)
            {
                throw new ArgumentException(nameof(model.UserId));
            }

            var book = await this.data.Books
                .FirstOrDefaultAsync(b => b.Id == model.BookId);
            if (book is null)
            {
                throw new ArgumentException(nameof(model.BookId));
            }

            var userBook = user.ApplicationUsersBooks.FirstOrDefault(ub => ub.BookId == model.BookId);
            if (userBook is not null)
            {
                user.ApplicationUsersBooks.Remove(userBook);
                await this.data.SaveChangesAsync();
            }
        }
    }
}
