using Bookstore.ApiTests.Models;
using System.Net.Http.Json;

namespace Bookstore.ApiTests.Clients;

public class BooksClient : BaseClient
{
    public BooksClient(HttpClient httpClient) : base(httpClient) { }

    public async Task<HttpResponseMessage> GetAllBooksAsync() => await ExecuteAndCapture<HttpResponseMessage>(async () =>
    {
        return await _httpClient.GetAsync("/api/v1/Books");
    });

    public async Task<HttpResponseMessage> GetBookByIdAsync(int id) => await ExecuteAndCapture<HttpResponseMessage>(async () =>
    {
        return await _httpClient.GetAsync($"/api/v1/Books/{id}");
    });

    public async Task<HttpResponseMessage> CreateBookAsync(Book book) => await ExecuteAndCapture<HttpResponseMessage>(async () =>
    {
        return await _httpClient.PostAsJsonAsync("/api/v1/Books", book);
    });
    public async Task<HttpResponseMessage> UpdateBookAsync(int id, Book book) => await ExecuteAndCapture<HttpResponseMessage>(async () =>
    {
        return await _httpClient.PutAsJsonAsync($"/api/v1/Books/{id}", book);
    });

    public async Task<HttpResponseMessage> DeleteBookAsync(string id) => await ExecuteAndCapture<HttpResponseMessage>(async () =>
    {
        return await _httpClient.DeleteAsync($"/api/v1/Books/{id}");
    });
}

