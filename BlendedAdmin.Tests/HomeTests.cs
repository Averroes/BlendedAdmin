using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Hosting;
using BlendedAdmin;
using Microsoft.AspNetCore.TestHost;
using System.Threading.Tasks;

namespace BlendedAdmin.Tests
{
    [TestClass]
    public class HomeTests
    {
        [TestMethod]
        public async Task HomePageIsLoaded()
        {
            var webHostBuilder =
                  new WebHostBuilder()
                        .UseEnvironment("Test") // You can set the environment you want (development, staging, production)
                        .UseStartup<Startup>(); // Startup class of your web app project

            using (var server = new TestServer(webHostBuilder))
            using (var client = server.CreateClient())
            {
                string result = await client.GetStringAsync("/");
                Assert.AreEqual("[\"value1\",\"value2\"]", result);
            }
        }
    }
}
