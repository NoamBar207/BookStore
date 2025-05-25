using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

[ApiController]
[Route("[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IBookService _bookService;

    public ReportsController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("/report")]
    public ContentResult GetReport()
    {
        var books = _bookService.Load().Books;

        var html = new StringBuilder();
        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html lang=\"en\">");
        html.AppendLine("<head>");
        html.AppendLine("<meta charset=\"UTF-8\">");
        html.AppendLine("<title>Book Report</title>");
        html.AppendLine("<style>");
        html.AppendLine(@"
            body {
                font-family: Arial, sans-serif;
                padding: 2rem;
                background-color: #f9f9f9;
            }
            h1 {
                text-align: center;
            }
            table {
                width: 100%;
                border-collapse: collapse;
                background-color: white;
            }
            th, td {
                padding: 12px;
                border: 1px solid #ddd;
                vertical-align: top;
            }
            th {
                background-color: #f1f1f1;
                text-align: left;
            }
            tr:nth-child(even) {
                background-color: #f9f9f9;
            }
            .authors {
                white-space: pre-line;
            }
        ");
        html.AppendLine("</style>");
        html.AppendLine("</head>");
        html.AppendLine("<body>");
        html.AppendLine("<h1>Bookstore Report</h1>");
        html.AppendLine("<table>");
        html.AppendLine("<thead>");
        html.AppendLine("<tr>");
        html.AppendLine("<th>Isbn</th>");
        html.AppendLine("<th>Title</th>");
        html.AppendLine("<th>Language</th>");
        html.AppendLine("<th>Author(s)</th>");
        html.AppendLine("<th>Category</th>");
        html.AppendLine("<th>Cover</th>");
        html.AppendLine("<th>Year</th>");
        html.AppendLine("<th>Price</th>");
        html.AppendLine("</tr>");
        html.AppendLine("</thead>");
        html.AppendLine("<tbody>");

        foreach (var book in books)
        {
            html.AppendLine("<tr>");
            html.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(book.Isbn)}</td>");
            html.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(book.Title.Text)}</td>");
            html.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(book.Title.Lang)}</td>");
            html.AppendLine($"<td class='authors'>{System.Net.WebUtility.HtmlEncode(string.Join(", ", book.Authors))}</td>");
            html.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(book.Category)}</td>");
            html.AppendLine($"<td>{System.Net.WebUtility.HtmlEncode(book.Cover)}</td>");
            html.AppendLine($"<td>{book.Year}</td>");
            html.AppendLine($"<td>${book.Price:F2}</td>");
            html.AppendLine("</tr>");
        }

        html.AppendLine("</tbody>");
        html.AppendLine("</table>");
        html.AppendLine("</body>");
        html.AppendLine("</html>");

        return new ContentResult
        {
            Content = html.ToString(),
            ContentType = "text/html",
            StatusCode = 200
        };
    }
}