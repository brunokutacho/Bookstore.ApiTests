
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Bookstore.ApiTests.Helpers;

static class ConfigManager
{
    public static int Retries { get; set; }
    public static int RetryDelayMs { get; set; }
    public static string Schema { get; set; }
    public static string BaseUrl { get; set; }
    public static IConfiguration AppSettings { get; set; }

    static ConfigManager()
    {
        LoadAppSettings();
        LoadVariables();

    }

    private static void LoadAppSettings()
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                   ?? throw new InvalidOperationException();

        AppSettings = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }

    private static void LoadVariables()
    {
        Retries = int.Parse(AppSettings["RETRIES"] ?? "3");
        RetryDelayMs = int.Parse(AppSettings["RETRY_DELAY_MS"] ?? "1000");
        Schema = AppSettings["SCHEMA"] ?? "http";
        BaseUrl = AppSettings["BASE_URL"] ?? "localhost:5000";
    }

    public static IReadOnlyDictionary<string, string> AsDictionary()
    {
        return AppSettings.AsEnumerable()
                          .Where(kv => kv.Value != null)
                          .ToDictionary(kv => kv.Key, kv => kv.Value!);
    }

}
