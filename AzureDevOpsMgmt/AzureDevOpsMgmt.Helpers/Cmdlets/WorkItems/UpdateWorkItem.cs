﻿// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : joirwi
// Created          : 03-20-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-20-2019
// ***********************************************************************
// <copyright file="UpdateWorkItem.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace AzureDevOpsMgmt.Cmdlets.WorkItems
{
    using System;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Helpers;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    using RestSharp;

    /// <summary>
    /// Class UpdateWorkItem.
    /// Implements the <see cref="AzureDevOpsMgmt.Cmdlets.PSCmdletPrivateBase" />
    /// </summary>
    /// <seealso cref="AzureDevOpsMgmt.Cmdlets.PSCmdletPrivateBase" />
    [Cmdlet(VerbsData.Update, "WorkItem")]
    public class UpdateWorkItem : PSCmdletPrivateBase
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Parameter(Mandatory = true)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the updated work item.
        /// </summary>
        /// <value>The updated work item.</value>
        [Parameter(Mandatory = true)]
        public WorkItem UpdatedWorkItem { get; set; }

        /// <summary>
        /// Gets or sets the original work item.
        /// </summary>
        /// <value>The original work item.</value>
        [Parameter]
        public WorkItem OriginalWorkItem { get; set; }

        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        /// <exception cref="T:System.Management.Automation.PipelineStoppedException">The pipeline has already been terminated, or was terminated
        ///             during the execution of this method.
        ///             The Cmdlet should generally just allow PipelineStoppedException
        ///             to percolate up to the caller of ProcessRecord etc.
        /// </exception>
        // ReSharper disable once StyleCop.SA1650
        protected override void ProcessRecord()
        {
            var request = new RestRequest($"wit/workitems/{this.Id}", Method.PATCH);
            request.AddOrUpdateParameter("Content-Type", "application/json-patch+json", ParameterType.HttpHeader);

            if (this.OriginalWorkItem == null)
            {
                var getRequest = new RestRequest($"wit/workitems/{this.Id}", Method.GET);
                var getResponse = this.client.Execute<WorkItem>(getRequest);

                if (getResponse.IsSuccessful)
                {
                    this.OriginalWorkItem = getResponse.Data;
                }
                else
                {
                    this.WriteError(new ErrorRecord(new Exception("Unable to retrieve original work items"), "AzureDevOpsMgmt.Cmdlets.UpdateWorkItem.FetchWorkItem.UnknownFailure", ErrorCategory.NotSpecified, $"Workitem: {this.Id}"));
                }
            }

            var patchDocument = JsonHelpers.CreatePatch(this.OriginalWorkItem, this.UpdatedWorkItem);

            request.AddJsonBody(patchDocument);
            this.client.Execute(request);
        }
    }
}
