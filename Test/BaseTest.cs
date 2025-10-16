using Allure.NUnit;
using Allure.NUnit.Attributes;
using Bookstore.ApiTests.Helpers;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using System.Reflection;

namespace Bookstore.ApiTests.Tests;

[AllureNUnit]
[AllureSuite("Bookstore API")]
public abstract class BaseTest
{
    protected static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
    private static readonly string LogDirectoryPath =
        Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, @"Logs/");
    public static readonly object Lock = new();
    public static int Retries => ConfigManager.Retries;
    public static int RetryDelayMs => ConfigManager.RetryDelayMs;

    public static IReadOnlyDictionary<string, string> Env => ConfigManager.AsDictionary();

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        lock (Lock)
        {
            try
            {
                Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            }
            catch (Exception ex)
            {
                var message = $"Error during OneTimeSetUp: {ex.Message}\n{ex.InnerException}";
                Logger.Error(ex, message);
                Console.WriteLine(message);
            }
        }
    }


    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status is TestStatus.Failed or TestStatus.Warning or TestStatus.Skipped)
        {
            _ = new Exception($"{TestContext.CurrentContext.Result.Message}\n\n{TestContext.CurrentContext.Result.StackTrace}");
        }
    }
}
