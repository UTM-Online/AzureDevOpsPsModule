// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 06-26-2019
// ***********************************************************************
// <copyright file="GetRelease.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Cmdlets.ReleaseManagement
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;

    using RestSharp;

    /// <summary>
    /// Class GetRelease.
    /// Implements the <see cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    /// </summary>
    /// <seealso cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    [Cmdlet(VerbsCommon.Get, "Release")]
    public class GetRelease : ApiCmdlet
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Parameter(ParameterSetName = "UserInput")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the definition identifier.
        /// </summary>
        /// <value>The definition identifier.</value>
        [Parameter(Mandatory = true, ParameterSetName = "UserInput")]
        public int DefinitionId { get; set; }

        /// <summary>
        /// Gets or sets the pipeline input.
        /// </summary>
        /// <value>The pipeline input.</value>
        [Parameter(ParameterSetName = "PipelineInput", DontShow = true, ValueFromPipeline = true)]
        public ReleaseDefinition PipelineInput { get; set; }

        /// <summary>
        /// Begins the processing cmdlet.
        /// </summary>
        protected override void BeginProcessingCmdlet()
        {
            if (this.PipelineInput != null)
            {
                this.DefinitionId = this.PipelineInput.Id;
            }
        }

        /// <summary>
        /// Processes the cmdlet record.
        /// </summary>
        protected override void ProcessCmdletRecord()
        {
            if (this.Id != default(int))
            {
                this.GetSingleRelease();
            }
            else
            {
                this.ListAllReleasesForDefinition();
            }
        }

        /// <summary>
        /// Gets the single release.
        /// </summary>
        private void GetSingleRelease()
        {
            var request = new RestRequest($"release/releases/{this.Id}");

            var response = this.Client.Get<Release>(request);

            this.WriteObject(response, DevOpsModelTarget.Release, ErrorCategory.NotSpecified, this);
        }

        /// <summary>
        /// Lists all releases for definition.
        /// </summary>
        private void ListAllReleasesForDefinition()
        {
            var request = new RestRequest("release/releases");
            request.AddQueryParameter("definitionId", this.DefinitionId.ToString());

            var response = this.Client.Get<List<Release>>(request);

            this.WriteObject(response, DevOpsModelTarget.Release, ErrorCategory.NotSpecified, this);
        }
    }
}
