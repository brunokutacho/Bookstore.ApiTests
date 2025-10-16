using Allure.Net.Commons;
using NUnit.Framework;

namespace Bookstore.ApiTests.Helpers;

[SetUpFixture]
public class AllureSetup
{
    [OneTimeSetUp]
    public void CleanAllureResults()
    {
        AllureLifecycle.Instance.CleanupResultDirectory();
    }
}
