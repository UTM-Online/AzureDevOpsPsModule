// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : joirwi
// Created          : 03-19-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-19-2019
// ***********************************************************************
// <copyright file="GetWorkItem.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AzureDevOpsMgmt.Cmdlets.WorkItems
{
    using System.Management.Automation;

    using AzureDevOpsMgmt.Helpers;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    using RestSharp;

    /// <summary>
    /// Class GetWorkItem.
    /// Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    [Cmdlet(VerbsCommon.Get, "WorkItem")]
    public class GetWorkItem : PSCmdlet
    {
        /// <summary>
        /// The client
        /// </summary>
        private RestClient client;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Parameter]
        public long Id { get; set; }

        /// <summary>
        /// When overridden in the derived class, performs initialization
        /// of command execution.
        /// Default implementation in the base class just returns.
        /// </summary>
        protected override void BeginProcessing()
        {
            this.client = this.GetRestClient();
        }

        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        /// <exception cref="T:System.Management.Automation.PipelineStoppedException">The pipeline has already been terminated, or was terminated
        ///             during the execution of this method.
        ///             The Cmdlet should generally just allow PipelineStoppedException
        ///             to percolate up to the caller of ProcessRecord etc.
        /// </exception>
        protected override void ProcessRecord()
        {
            var workItemRequest = new RestRequest($"wit/workitems/{this.Id}", Method.GET);
            var workItemResponse = this.client.Execute<WorkItem>(workItemRequest);

            if (workItemResponse.IsSuccessful)
            {
                this.WriteObject(workItemResponse.Data);
            }
            else
            {
                this.WriteObject("Request Failed.  Review Request / Response Variables");
                this.WriteVerbose($"BaseUri: {this.client.BaseUrl.OriginalString}");
                this.SetPsVariable("WorkItemRequestBody", workItemRequest);
                this.SetPsVariable("WorkItemResponseBody", workItemResponse);
            }
        }
    }
}
