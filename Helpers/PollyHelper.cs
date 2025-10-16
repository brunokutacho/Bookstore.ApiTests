using Polly;
using Polly.Retry;
using System.Net;

namespace Bookstore.ApiTests.Helpers;

public class PollyHelper
{
    public static int _retries { get; set; }
    public static int _retryDelayMs { get; set; }
    public AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
    private int _retryCount;
    protected async Task<HttpResponseMessage> ExecuteAndCapture<T>(Func<Task<HttpResponseMessage>> action)
    {
        _retryCount = 0;
        var policyResult = await _retryPolicy.ExecuteAndCaptureAsync(action);
        if (policyResult.Outcome == OutcomeType.Failure)
        {
            throw policyResult.FinalException;
        }
        return policyResult.Result;
    }

    public AsyncRetryPolicy<HttpResponseMessage> InitRetriablePolicy()
    {
        return Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r =>
                (!r.IsSuccessStatusCode && (int)r.StatusCode >= 500) ||
                r.StatusCode == HttpStatusCode.RequestTimeout)
            .WaitAndRetryAsync(
                _retries,
                retryAttempt => TimeSpan.FromMilliseconds(_retryDelayMs * retryAttempt),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    if (outcome.Exception != null)
                        Console.WriteLine($"Exception: {outcome.Exception.Message}");
                    else
                        Console.WriteLine($"Status: {outcome.Result.StatusCode}");
                });
    }
}
