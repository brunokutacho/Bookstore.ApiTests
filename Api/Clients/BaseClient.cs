using Bookstore.ApiTests.Helpers;

namespace Bookstore.ApiTests.Clients;

public class BaseClient : PollyHelper
{
    protected readonly HttpClient _httpClient;
    protected readonly string _baseUrl;

    public BaseClient(HttpClient httpClient)
    {
        _baseUrl = $"{ConfigManager.Schema}://{ConfigManager.BaseUrl}";
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_baseUrl);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _retries = ConfigManager.Retries;
        _retryDelayMs = ConfigManager.RetryDelayMs;
        _retryPolicy = InitRetriablePolicy();
    }
}

