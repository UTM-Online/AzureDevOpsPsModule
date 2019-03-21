using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureDevOpsMgmt.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOpsMgmt.Helpers.Tests
{
    using AzureDevOpsMgmt.CoreTests;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.WebApi.Patch;

    using Newtonsoft.Json;

    [TestClass()]
    public class JsonHelpersTests
    {
        [TestMethod()]
        public void CreatePatchTest()
        {
            var original = JsonConvert.DeserializeObject<WorkItem>(TestResources.JsonPatchTestSideA);
            var update = JsonConvert.DeserializeObject<WorkItem>(TestResources.JsonPatchSideB);

            var patchDoc = JsonHelpers.CreatePatch(original, update);

            Assert.AreEqual("5.9166", patchDoc.First(p => p.Operation == Operation.Replace && p.Path == "/Fields/Microsoft.VSTS.Scheduling.RemainingWork").Value.ToString());
            Assert.AreEqual("0.0833", patchDoc.First(p => p.Operation == Operation.Replace && p.Path == "/Fields/Microsoft.VSTS.Scheduling.CompletedWork").Value.ToString());
        }
    }
}