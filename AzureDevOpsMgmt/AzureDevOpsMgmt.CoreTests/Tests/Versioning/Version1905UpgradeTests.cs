// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.CoreTests
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="Version1905UpgradeTests.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.CoreTests.Tests.Versioning
{
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using AzureDevOpsMgmt.CoreTests.Resources;
    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    /// <summary>
    /// Summary Version1905UpgradeTests
    /// </summary>
    [TestClass]
    public class Version1905UpgradeTests
    {
        /// <summary>
        /// The test context instance.
        /// </summary>
        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get
            {
                return this.testContextInstance;
            }

            set
            {
                this.testContextInstance = value;
            }
        }

        #region Additional test attributes
        // You can use the following additional attributes as you write your tests:
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        #endregion

        /// <summary>
        /// The account not on machine test.
        /// </summary>
        [TestMethod]
        public void AccountNotOnMachineTest()
        {
            var accountCollection = JsonConvert.DeserializeObject<AzureDevOpsAccountCollection>(TestResources.Pre1905TestConfigFile);
            var token = accountCollection.PatTokens.First();

            ModuleStartup.ProcessCredentials(token);

            Assert.IsFalse(token.IsInScope);
            Debug.Write(ConfigurationHelpers.GetMachineId());
        }

        /// <summary>
        /// The account on machine test.
        /// </summary>
        [TestMethod]
        public void AccountOnMachineTest()
        {
            var accountCollection = JsonConvert.DeserializeObject<AzureDevOpsAccountCollection>(TestResources.TestConfigFile);
            var token = accountCollection.PatTokens.First();

            ModuleStartup.ProcessCredentials(token);

            Assert.IsTrue(token.IsInScope);
        }
    }
}
