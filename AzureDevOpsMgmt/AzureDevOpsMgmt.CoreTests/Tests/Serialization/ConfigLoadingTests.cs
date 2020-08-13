// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.CoreTests
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="ConfigLoadingTests.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.CoreTests.Tests.Serialization
{
    using System;
    using System.Linq;

    using AzureDevOpsMgmt.CoreTests.Resources;
    using AzureDevOpsMgmt.Models;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    /// <summary>
    /// The config loading tests.
    /// </summary>
    [TestClass]
    public class ConfigLoadingTests
    {
        /// <summary>
        /// The load test config test.
        /// </summary>
        [TestMethod]
        public void LoadTestConfigTest()
        {
            var accountObject = JsonConvert.DeserializeObject<AzureDevOpsAccountCollection>(TestResources.TestConfigFile);

            Assert.IsNotNull(accountObject);
            Assert.IsFalse(accountObject.PatTokens.Any(p => p.Id == Guid.Empty));
        }
    }
}
