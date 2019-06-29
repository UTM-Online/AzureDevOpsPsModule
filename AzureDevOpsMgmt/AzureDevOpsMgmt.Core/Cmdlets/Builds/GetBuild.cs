// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 06-26-2019
// ***********************************************************************
// <copyright file="GetBuild.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Cmdlets.Builds
{
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    using Microsoft.TeamFoundation.Build.WebApi;

    using RestSharp;

    /// <summary>
    /// Class GetBuild.
    /// Implements the <see cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    /// </summary>
    /// <seealso cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    [Cmdlet(VerbsCommon.Get, "Build", DefaultParameterSetName = "BuildDefinitionList")]
    public class GetBuild : ApiCmdlet
    {
        /// <summary>
        /// Gets or sets the build identifier.
        /// </summary>
        /// <value>The build identifier.</value>
        [Parameter(ParameterSetName = "SingleBuild")]
        public int BuildId { get; set; }

        /// <summary>
        /// Gets or sets the build definition identifier.
        /// </summary>
        /// <value>The build definition identifier.</value>
        [Parameter(ParameterSetName = "BuildDefinitionList")]
        public int[] BuildDefinitionId { get; set; }

        /// <summary>
        /// Processes the cmdlet record.
        /// </summary>
        protected override void ProcessCmdletRecord()
        {
            var request = new RestRequest("build/builds", Method.GET);

            IRestResponse<ResponseModel<Build>> result;

            if (this.ParameterSetName == "BuildDefinitionList" && this.BuildDefinitionId == null)
            {
                result = this.Client.Get<ResponseModel<Build>>(request);
            }
            else if (this.ParameterSetName == "BuildDefinitionList")
            {
                request.AddParameter("definitions", this.BuildDefinitionId, ParameterType.QueryString);
                result = this.Client.Get<ResponseModel<Build>>(request);
            }
            else
            {
                request.Resource += $"/{this.BuildId}";
                result = this.Client.Get<ResponseModel<Build>>(request);
            }

            this.WriteObject(result, DevOpsModelTarget.Build, ErrorCategory.NotSpecified, this);
        }
    }
}
