using Bogus;
using Bookstore.ApiTests.Models;

namespace Bookstore.ApiTests.Data;

public static class TestDataFactory
{
    private static readonly Faker _faker = new();

    public static Author GenerateAuthor(int? id = null, int? idBook = null)
    {
        return new Faker<Author>()
            .RuleFor(a => a.Id, f => id ?? f.Random.Int(1, 10000))
            .RuleFor(a => a.IdBook, f => idBook ?? f.Random.Int(1, 100))
            .RuleFor(a => a.FirstName, f => f.Name.FirstName())
            .RuleFor(a => a.LastName, f => f.Name.LastName())
            .Generate();
    }

    public static Book GenerateBook(int? id = null)
    {
        return new Faker<Book>()
            .RuleFor(b => b.Id, f => id ?? f.Random.Int(1, 10000))
            .RuleFor(b => b.Title, f => f.Lorem.Sentence(3))
            .RuleFor(b => b.Description, f => f.Lorem.Paragraph())
            .RuleFor(b => b.PageCount, f => f.Random.Int(50, 500))
            .RuleFor(b => b.Excerpt, f => f.Lorem.Sentence(5))
            .RuleFor(b => b.PublishDate, f => f.Date.Past(5).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
            .Generate();
    }
}
