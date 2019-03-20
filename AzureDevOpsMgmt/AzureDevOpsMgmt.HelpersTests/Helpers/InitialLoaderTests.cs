using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureDevOpsMgmt.Helpers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsMgmt.Helpers.Helpers.Tests
{
    [TestClass()]
    public class InitialLoaderTests
    {
        [TestMethod()]
        public void LoadConfigurationTest()
        {
            var testItem = InitialLoader.LoadConfiguration();
            Assert.IsNotNull(testItem);
        }
    }
}