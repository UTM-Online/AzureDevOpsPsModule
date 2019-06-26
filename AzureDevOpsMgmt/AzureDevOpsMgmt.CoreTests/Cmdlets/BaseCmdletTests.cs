using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AzureDevOpsMgmt.CoreTests.Cmdlets
{
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using System.Reflection;

    using AzureDevOpsMgmt.Cmdlets;
    using AzureDevOpsMgmt.CoreTests.Models;

    using RestSharp;

    [TestClass]
    public class BaseCmdletTests
    {
        [TestMethod]
        public void VerifyDiFunctionTest()
        {
            IRestClient results = null;

            var moduleBaseAssembly = Assembly.GetAssembly(typeof(ApiCmdlet));
            var testBaseAssembly = Assembly.GetAssembly(typeof(BaseCmdletTests));

            using (var ps = PowerShell.Create(RunspaceMode.NewRunspace))
            {
                using (var importBaseModule = ps.CreateNestedPowerShell())
                {
                    importBaseModule.AddCommand("Import-Module");
                    importBaseModule.AddParameter("Assembly", moduleBaseAssembly);
                }
            }

            Assert.IsNotNull(results);
        }
    }
}
