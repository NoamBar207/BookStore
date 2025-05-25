using System.Globalization;
using System.Xml.Serialization;

namespace BookStoreApi.Models
{
    [XmlRoot("bookstore")]
    public class BookStore
    {
        [XmlElement("book")]
        public List<Book> Books { get; set; } = new();
    }
    
    [XmlRoot("book")]
    public class Book
    {
        [XmlElement("isbn")]
        public string Isbn { get; set; } = string.Empty;

        [XmlElement("title")]
        public required Title Title { get; set; } = new Title();

        [XmlAttribute("category")]
        public string Category { get; set; } = string.Empty;

        [XmlAttribute("cover")]
        public string? Cover { get; set; }

        [XmlElement("author")]
        public List<string> Authors  { get; set; } = new List<string>();

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("year")]
        public int Year { get; set; }
    }


    public class Title
    {
        [XmlAttribute("lang")]
        public string Lang { get; set; } = string.Empty;

        [XmlText]
        public string Text { get; set; } = string.Empty;
    }
}
