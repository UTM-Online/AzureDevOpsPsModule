namespace AzureDevOpsMgmt.CoreTests.Serialization
{
    using System;
    using System.IO;
    using System.Linq;

    using AzureDevOpsMgmt.Exceptions;
    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;
    using AzureDevOpsMgmt.Resources;

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

        [TestMethod]
        public void VerifyEmtyGuidIsntWritten()
        {
            var accountObject = JsonConvert.DeserializeObject<AzureDevOpsAccountCollection>(TestResources.TestConfigFile);

            accountObject.PatTokens.First().Id = Guid.Empty;

            accountObject.PatTokens.First(p => p.Id != null).Id = Guid.Empty;

            var tempFilePath = Path.GetTempFileName();

            Assert.ThrowsException<EmptyIdFoundException>(() => FileHelpers.WriteFileJson(tempFilePath, accountObject));

            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }
}
