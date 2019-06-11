namespace AzureDevOpsMgmt.CoreTests.Serialization
{
    using System;
    using System.Linq;

    using AzureDevOpsMgmt.Models;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    [TestClass]
    public class ConfigLoadingTests
    {
        [TestMethod]
        public void LoadTestConfigTest()
        {
            var accountObject = JsonConvert.DeserializeObject<AzureDevOpsAccountCollection>(TestResources.TestConfigFile);

            Assert.IsNotNull(accountObject);
            Assert.IsFalse(accountObject.PatTokens.Any(p => p.Id == Guid.Empty));
        }
    }
}
