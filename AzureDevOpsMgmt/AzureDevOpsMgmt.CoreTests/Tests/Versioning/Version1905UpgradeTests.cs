using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureDevOpsMgmt.CoreTests.Tests.Versioning
{
    using System.Diagnostics;
    using System.Linq;

    using AzureDevOpsMgmt.Cmdlets.Startup;
    using AzureDevOpsMgmt.CoreTests.Resources;
    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;

    using Newtonsoft.Json;

    /// <summary>
    /// Summary description for Version1905UpgradeTests
    /// </summary>
    [TestClass]
    public class Version1905UpgradeTests
    {
        public Version1905UpgradeTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void AccountNotOnMachineTest()
        {
            var accountCollection = JsonConvert.DeserializeObject<AzureDevOpsAccountCollection>(TestResources.Pre1905TestConfigFile);
            var token = accountCollection.PatTokens.First();

            ImportConfiguration.ProcessCredentials(token);

            Assert.IsFalse(token.IsInScope);
            Debug.Write(ConfigurationHelpers.GetMachineId());
        }

        [TestMethod]
        public void AccountOnMachineTest()
        {
            var accountCollection = JsonConvert.DeserializeObject<AzureDevOpsAccountCollection>(TestResources.TestConfigFile);
            var token = accountCollection.PatTokens.First();

            ImportConfiguration.ProcessCredentials(token);

            Assert.IsTrue(token.IsInScope);
        }
    }
}
