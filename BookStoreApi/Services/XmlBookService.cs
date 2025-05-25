using System.IO;
using System.Linq;
using System.Xml.Serialization;
using BookStoreApi.Models;
using BookStoreApi.Models.Dtos;

namespace BookStoreApi.Services
{
    public class XmlBookService : IBookService
    {
        private readonly string _filePath;
        public XmlBookService(string filePath)
        {
            _filePath = filePath;
        }

        public BookStore Load()
        {
            using var stream = File.OpenRead(_filePath);
            var serializer = new XmlSerializer(typeof(BookStore));
            var bookstore = (BookStore)serializer.Deserialize(stream)!;
            return bookstore;
        }

        public void Save(BookStore store)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(BookStore));
            using FileStream stream = new FileStream(_filePath, FileMode.Create);
            serializer.Serialize(stream, store);
        }

        public bool Add(Book newBook)
        {
            var store = Load();
            if (store.Books.Any(b => b.Isbn == newBook.Isbn)) return false;
            store.Books.Add(newBook);
            Save(store);
            return true;
        }

        public bool Delete(string isbn)
        {
            var store = Load();
            var book = store.Books.FirstOrDefault(b => b.Isbn == isbn);
            if (book == null) return false;
            store.Books.Remove(book);
            Save(store);
            return true;
        }

        public bool Update(string isbn, BookUpdateDto updatedBook)
        {
            var store = Load();
            var existingBook = store.Books.FirstOrDefault(b => b.Isbn == isbn);
            if (existingBook == null) return false;
            
            static bool IsValidString(string value) => !string.IsNullOrWhiteSpace(value) && value?.ToLower() != "string";
            existingBook.Title.Text = IsValidString(updatedBook.Title?.Text) ? updatedBook.Title.Text : existingBook.Title.Text;
            existingBook.Title.Lang = IsValidString(updatedBook.Title?.Lang) ? updatedBook.Title.Lang : existingBook.Title.Lang;
            existingBook.Category = IsValidString(updatedBook.Category) ? updatedBook.Category : existingBook.Category;
            existingBook.Cover = IsValidString(updatedBook.Cover)  ? updatedBook.Cover : existingBook.Cover;

            if (updatedBook.Authors?.Count > 0)
            {
                var validAuthors = updatedBook.Authors.Where(author => IsValidString(author)).ToList();
                if (validAuthors.Count > 0)
                {
                    existingBook.Authors = validAuthors;
                }
            }

            if (updatedBook.Year.HasValue && updatedBook.Year.Value > 0) existingBook.Year = updatedBook.Year.Value;
            if (updatedBook.Price.HasValue && updatedBook.Price.Value > 0) existingBook.Price = updatedBook.Price.Value;

            Save(store);
            return true;
        }

        public Book? GetBookByIsbn(string isbn)
        {
            var store = Load();
            return store.Books.FirstOrDefault(b => b.Isbn == isbn);
        }
    }
}