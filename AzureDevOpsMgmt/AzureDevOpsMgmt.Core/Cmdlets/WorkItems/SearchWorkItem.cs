// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : josh
// Created          : 07-08-2019
// ***********************************************************************
// <copyright file="SearchWorkItem.cs" company="UTM-Online">
//     Copyright ©  2019
// </copyright>
// <summary>This cmdlet enables the user to search for work items using the "Work Item Query Language"</summary>
// ***********************************************************************

namespace AzureDevOpsMgmt.Cmdlets.WorkItems
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    using RestSharp;

    /// <summary>
    /// Class SearchWorkItem.
    /// Implements the <see cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    /// Implements the <see cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    /// </summary>
    /// <seealso cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    [Cmdlet(VerbsCommon.Search, "WorkItems")]
    public class SearchWorkItem : ApiCmdlet
    {
        /// <summary>
        /// The work items
        /// </summary>
        private readonly List<WorkItem> workItems = new List<WorkItem>();

        /// <summary>
        /// The query results
        /// </summary>
        private WorkItemQueryResult queryResults;

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>The query.</value>
        [Parameter]
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        /// <inheritdoc />
        [ShouldInject("AdoTeamsApi")]
        protected override IRestClient Client { get; set; }

        /// <summary>
        /// Ends the cmdlet processing.
        /// </summary>
        protected override void EndCmdletProcessing()
        {
            if (this.workItems.Any())
            {
                this.WriteObject(this.workItems, true);
            }
            else
            {
                this.WriteWarning("Query returned no results!");
            }
        }

        /// <summary>
        /// Processes the cmdlet record.
        /// </summary>
        protected override void ProcessCmdletRecord()
        {
            var request = new RestRequest("/wit/wiql");

            request.AddJsonBody(this.Query);

            var response = this.Client.Post<WorkItemQueryResult>(request);

            if (!response.IsSuccessful)
            {
                this.ThrowTerminatingError(new ErrorRecord(response.ErrorException, this.BuildStandardErrorId(DevOpsModelTarget.WorkItem), ErrorCategory.NotSpecified, response));
            }

            this.queryResults = response.Data;

            var queryFields = string.Join(",", this.queryResults.Columns.Select(i => i.Name));

            foreach (var item in this.queryResults.WorkItems)
            {
                var arguments = new Dictionary<string, object> { { "Id", item.Id }, { "Fields", queryFields } };
                var workItem = this.InvokeModuleCmdlet<WorkItem>("Get-WorkItem", arguments).First();

                this.workItems.Add(workItem);
            }
        }
    }
}