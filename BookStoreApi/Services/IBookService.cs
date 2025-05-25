using BookStoreApi.Models; 
using BookStoreApi.Models.Dtos;

namespace BookStoreApi.Services
{
    public interface IBookService
    {
        BookStore Load();
        Book GetBookByIsbn(string isbn);
        bool Add(Book book);
        bool Update(string isbn, BookUpdateDto updatedBook);
        bool Delete(string isbn);
    }
}