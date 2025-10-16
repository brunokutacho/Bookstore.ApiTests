using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using Bookstore.ApiTests.Clients;
using Bookstore.ApiTests.Data;
using Bookstore.ApiTests.Models;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace Bookstore.ApiTests.Tests;

[AllureNUnit]
[AllureSuite("Books API")]
[TestFixture]
[Author("Bruno Kutacho")]
[Category("API")]
public class BooksTests
{
    private BooksClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new BooksClient(new HttpClient());
    }

    // -------------------- SUCCESS TESTS --------------------

    [Test]
    [AllureFeature("Books")]
    [AllureStory("Retrieve all books")]
    [TestCase(TestName = "Get all books successfully")]
    public async Task GetAllBooks_Success()
    {
        await AllureApi.Step("Request all books", async () =>
        {
            var response = await _client.GetAllBooksAsync();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var books = await response.Content.ReadFromJsonAsync<List<Book>>();
            Assert.That(books, Is.Not.Null);
            Assert.That(books!.Count, Is.GreaterThan(0));
        });
    }

    [Test]
    [AllureFeature("Books")]
    [AllureStory("Retrieve book by ID")]
    [TestCase(1, TestName = "Get book by ID 1 successfully")]
    public async Task GetBookById_Success(int bookId)
    {
        await AllureApi.Step($"Request book with ID {bookId}", async () =>
        {
            var response = await _client.GetBookByIdAsync(bookId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var book = await response.Content.ReadFromJsonAsync<Book>();
            Assert.That(book, Is.Not.Null);
            Assert.That(book!.Id, Is.EqualTo(bookId));
        });
    }

    [Test]
    [AllureFeature("Books")]
    [AllureStory("Create new book")]
    [TestCase(TestName = "Create new random book successfully")]
    public async Task CreateBook_Success()
    {
        var newBook = TestDataFactory.GenerateBook();

        await AllureApi.Step("Send create book request", async () =>
        {
            var response = await _client.CreateBookAsync(newBook);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK)
                .Or.EqualTo(HttpStatusCode.Created));

            var created = await response.Content.ReadFromJsonAsync<Book>();
            Assert.That(created!.Title, Is.EqualTo(newBook.Title));

            var json = System.Text.Json.JsonSerializer.Serialize(created);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            AllureApi.AddAttachment("Created Book Payload", "application/json", bytes, "json");
        });
    }

    [Test]
    [AllureFeature("Books")]
    [AllureStory("Update book by ID")]
    [TestCase(TestName = "Update book ID 9999 successfully")]
    public async Task UpdateBook_Success()
    {
        var updatedBook = TestDataFactory.GenerateBook(9999);

        await AllureApi.Step($"Send update request for book {updatedBook.Id}", async () =>
        {
            var response = await _client.UpdateBookAsync(updatedBook.Id, updatedBook);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var book = await response.Content.ReadFromJsonAsync<Book>();
            Assert.That(book!.Title, Is.EqualTo(updatedBook.Title));
        });
    }

    [Test]
    [AllureFeature("Books")]
    [AllureStory("Delete book by ID")]
    [TestCase(TestName = "Delete book ID 9999 successfully")]
    public async Task DeleteBook_Success()
    {
        int bookId = 9999;

        await AllureApi.Step($"Send delete request for book {bookId}", async () =>
        {
            var response = await _client.DeleteBookAsync(bookId.ToString());
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        });
    }

    // -------------------- FAILURE TESTS --------------------

    [Test]
    [AllureFeature("Books")]
    [AllureStory("Get non-existent book")]
    [TestCase(999999, TestName = "Get non-existent book returns 404")]
    public async Task GetBookById_NotFound(int bookId)
    {
        await AllureApi.Step($"Request non-existent book {bookId}", async () =>
        {
            var response = await _client.GetBookByIdAsync(bookId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        });
    }


    [Test]
    [AllureFeature("Books")]
    [AllureStory("Delete invalid book")]
    [TestCase("99999999999", TestName = "Delete invalid book returns 400")]
    public async Task DeleteBook_NotFound(string bookId)
    {
        await AllureApi.Step($"Attempt to delete invalid book {bookId}", async () =>
        {
            var response = await _client.DeleteBookAsync(bookId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        });
    }
}
