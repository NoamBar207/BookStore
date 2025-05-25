using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Models;
using BookStoreApi.Services;
using BookStoreApi.Models.Dtos;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;

        public BooksController(IBookService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var bookstore = _service.Load();
            return Ok(bookstore.Books);
        }

        [HttpGet("{isbn}")]
        public IActionResult GetBookByIsbn(string isbn)
        {
            var book = _service.GetBookByIsbn(isbn);
            return book == null ? NotFound("book not found") : Ok(book);
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] Book newBook)
        {
            var isAdded = _service.Add(newBook);
            if (!isAdded) return Conflict($"Book with ISBN '{newBook.Isbn}' already exists.");
            return CreatedAtAction(nameof(GetBookByIsbn), new { isbn = newBook.Isbn }, newBook);
        }

        [HttpDelete("{isbn}")]
        public IActionResult DeleteBook(string isbn)
        {
            var isDeleted = _service.Delete(isbn);
            return isDeleted ? NoContent() : NotFound("book not found");
        }
        
        [HttpPut("{isbn}")]
        public IActionResult UpdateBook(string isbn, [FromBody] BookUpdateDto updatedBook)
        {
            var isUpdated = _service.Update(isbn, updatedBook);
            return isUpdated ? NoContent() : NotFound("book not found");
        }
    }
}