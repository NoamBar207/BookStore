using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookStoreApi.Models;

namespace BookStoreApi.Models.Dtos
{
    public class BookUpdateDto
    {
        public Title Title { get; set; }
        public List<string>? Authors { get; set; } = new();
        public int? Year { get; set; }
        public decimal? Price { get; set; }
        public string? Category { get; set; }
        public string? Cover { get; set; }
    }
}