using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    /// <summary>
    /// Base end-to-end Controller test class.
    /// Notes:  Inheritance was used as the Initialization and CleanUp actions are shared amongst the 2 existing controllers.
    ///         Composition was considered with ITestController for the dependencies, but duplicated code would appear for
    ///         different controllers. To maintain simplicity and code re usage, inheritance was chosen.
    /// </summary>
    public class ControllerTestsBase
    {
        protected WebApplicationFactory<Program> WebAppFactory { get; private set; }
        protected HttpClient Client { get; private set; }

        [TestInitialize]
        public void TestInitialize()
        {
            WebAppFactory = new WebApplicationFactory<Program>();
            Client = WebAppFactory.CreateClient();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Client.Dispose();
            WebAppFactory.Dispose();
        }
    }
}
