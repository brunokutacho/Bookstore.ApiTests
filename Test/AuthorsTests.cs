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
[AllureSuite("Authors API")]
[AllureSubSuite("CRUD Tests")]
[TestFixture]
[Author("Bruno Kutacho")]
[Category("API")]
public class AuthorsTests
{
    private AuthorsClient _client;

    [SetUp]
    public void Setup()
    {
        _client = new AuthorsClient(new HttpClient());
    }

    // -------------------- SUCCESS TESTS --------------------

    [Test]
    [AllureTag("GET")]
    [AllureFeature("Authors")]
    [AllureStory("Retrieve all authors")]
    [TestCase(TestName = "Get all authors successfully")]
    public async Task GetAllAuthors_Success()
    {
        await AllureApi.Step("Request all authors", async () =>
        {
            var response = await _client.GetAllAuthorsAsync();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var authors = await response.Content.ReadFromJsonAsync<List<Author>>();
            Assert.That(authors, Is.Not.Null);
            Assert.That(authors!.Count, Is.GreaterThan(0));
        });
    }

    [Test]
    [AllureTag("GET")]
    [AllureFeature("Authors")]
    [AllureStory("Retrieve author by ID")]
    [TestCase(1, TestName = "Get author by ID 1 successfully")]
    public async Task GetAuthorById_Success(int authorId)
    {
        await AllureApi.Step("Request author by ID", async () =>
        {
            var response = await _client.GetAuthorByIdAsync(authorId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var author = await response.Content.ReadFromJsonAsync<Author>();
            Assert.That(author, Is.Not.Null);
            Assert.That(author!.Id, Is.EqualTo(authorId));
        });
    }

    [Test]
    [AllureTag("POST")]
    [AllureFeature("Authors")]
    [AllureStory("Create new author")]
    [TestCase(TestName = "Create new random author successfully")]
    public async Task CreateAuthor_Success()
    {
        var newAuthor = TestDataFactory.GenerateAuthor();

        await AllureApi.Step("Send create author request", async () =>
        {
            var response = await _client.CreateAuthorAsync(newAuthor);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK)
                .Or.EqualTo(HttpStatusCode.Created));

            var created = await response.Content.ReadFromJsonAsync<Author>();
            Assert.That(created!.FirstName, Is.EqualTo(newAuthor.FirstName));

            var json = System.Text.Json.JsonSerializer.Serialize(created);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            AllureApi.AddAttachment("Created Author Payload", "application/json", bytes, "json");
        });
    }

    [Test]
    [AllureTag("PUT")]
    [AllureFeature("Authors")]
    [AllureStory("Update author by ID")]
    [TestCase(TestName = "Update author ID 9999 successfully")]
    public async Task UpdateAuthor_Success()
    {
        var updatedAuthor = TestDataFactory.GenerateAuthor(9999);

        await AllureApi.Step("Send update author request", async () =>
        {
            var response = await _client.UpdateAuthorAsync(updatedAuthor.Id, updatedAuthor);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var author = await response.Content.ReadFromJsonAsync<Author>();
            Assert.That(author!.FirstName, Is.EqualTo(updatedAuthor.FirstName));
        });
    }

    [Test]
    [AllureTag("DELETE")]
    [AllureFeature("Authors")]
    [AllureStory("Delete author by ID")]
    [TestCase(TestName = "Delete author ID 9999 successfully")]
    public async Task DeleteAuthor_Success()
    {
        string authorId = "9999";

        await AllureApi.Step("Send delete author request", async () =>
        {
            var response = await _client.DeleteAuthorAsync(authorId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        });
    }

    // -------------------- FAILURE TESTS --------------------

    [Test]
    [AllureTag("NEGATIVE")]
    [AllureFeature("Authors")]
    [AllureStory("Get non-existent author")]
    [TestCase(999999, TestName = "Get non-existent author returns 404")]
    public async Task GetAuthorById_NotFound(int authorId)
    {
        await AllureApi.Step("Request non-existent author", async () =>
        {
            var response = await _client.GetAuthorByIdAsync(authorId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        });
    }

    [Test]
    [AllureTag("NEGATIVE")]
    [AllureFeature("Authors")]
    [AllureStory("Delete invalid author")]
    [TestCase("9999999999", TestName = "Delete invalid author returns 400")]
    public async Task DeleteAuthor_NotFound(string authorId)
    {
        await AllureApi.Step("Request delete invalid author", async () =>
        {
            var response = await _client.DeleteAuthorAsync(authorId);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        });
    }
}
