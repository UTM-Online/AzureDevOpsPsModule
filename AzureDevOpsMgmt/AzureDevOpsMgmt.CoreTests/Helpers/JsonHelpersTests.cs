// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.CoreTests
// Author           : Josh Irwin
// Created          : 06-07-2019
// ***********************************************************************
// <copyright file="JsonHelpersTests.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.CoreTests.Helpers
{
    using System.Linq;

    using AzureDevOpsMgmt.CoreTests;
    using AzureDevOpsMgmt.Helpers;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.WebApi.Patch;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    /// <summary>
    /// Defines test class JsonHelpersTests.
    /// </summary>
    [TestClass]
    public class JsonHelpersTests
    {
        /// <summary>
        /// Defines the test method CreatePatchTest.
        /// </summary>
        [TestMethod]
        public void CreatePatchTest()
        {
            var original = JsonConvert.DeserializeObject<WorkItem>(TestResources.JsonPatchTestSideA);
            var update = JsonConvert.DeserializeObject<WorkItem>(TestResources.JsonPatchSideB);

            var patchDoc = JsonHelpers.CreatePatch(original, update);

            Assert.AreEqual("5.9166", patchDoc.First(p => p.Operation == Operation.Replace && p.Path == "/fields/Microsoft.VSTS.Scheduling.RemainingWork").Value.ToString());
            Assert.AreEqual("0.0833", patchDoc.First(p => p.Operation == Operation.Replace && p.Path == "/fields/Microsoft.VSTS.Scheduling.CompletedWork").Value.ToString());
        }
    }
}