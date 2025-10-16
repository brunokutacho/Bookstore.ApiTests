using Bookstore.ApiTests.Models;
using System.Net.Http.Json;

namespace Bookstore.ApiTests.Clients;

public class AuthorsClient : BaseClient
{
    public AuthorsClient(HttpClient httpClient) : base(httpClient) { }

    public async Task<HttpResponseMessage> GetAllAuthorsAsync() => await ExecuteAndCapture<HttpResponseMessage>(async () =>
    {
        return await _httpClient.GetAsync("/api/v1/Authors");
    });
    public async Task<HttpResponseMessage> GetAuthorByIdAsync(int id) => await ExecuteAndCapture<HttpResponseMessage>(async () =>
    {
        return await _httpClient.GetAsync($"/api/v1/Authors/{id}");
    });
    public async Task<HttpResponseMessage> CreateAuthorAsync(Author author) => await ExecuteAndCapture<HttpResponseMessage>(async () =>
    {
        return await _httpClient.PostAsJsonAsync("/api/v1/Authors", author);
    });
    public async Task<HttpResponseMessage> UpdateAuthorAsync(int id, Author author) => await ExecuteAndCapture<HttpResponseMessage>(async () =>
    {
        return await _httpClient.PutAsJsonAsync($"/api/v1/Authors/{id}", author);
    });
    public async Task<HttpResponseMessage> DeleteAuthorAsync(string id) => await ExecuteAndCapture<HttpResponseMessage>(async () =>
    {
        return await _httpClient.DeleteAsync($"/api/v1/Authors/{id}");
    });

}
