using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureDevOpsMgmt.CoreTests.Cmdlets
{
    using System.Linq;
    using System.Management.Automation;

    using AzureDevOpsMgmt.CoreTests.Models;

    using RestSharp;

    [TestClass]
    public class BaseCmdletTests
    {
        [TestMethod]
        public void VerifyDiFunctionTest()
        {
            Cmdlet testCmdlet = new TestCmdlet();

            var results = testCmdlet.Invoke<IRestClient>()?.First();

            Assert.IsNotNull(results);
        }
    }
}
