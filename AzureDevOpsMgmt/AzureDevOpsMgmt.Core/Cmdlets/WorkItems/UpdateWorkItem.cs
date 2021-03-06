﻿// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="UpdateWorkItem.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************

namespace AzureDevOpsMgmt.Cmdlets.WorkItems
{
    using System.Management.Automation;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.WebApi.Patch;

    using Newtonsoft.Json;

    using RestSharp;

    /// <summary>
    /// Class UpdateWorkItem.
    /// Implements the <see cref="ApiCmdlet" />
    /// Implements the <see cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    /// </summary>
    /// <seealso cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    /// <seealso cref="ApiCmdlet" />
    [Cmdlet(VerbsData.Update, "WorkItem")]
    public class UpdateWorkItem : ApiCmdlet
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
        /// during the execution of this method.
        /// The Cmdlet should generally just allow PipelineStoppedException
        /// to percolate up to the caller of ProcessRecord etc.</exception>
        // ReSharper disable once StyleCop.SA1650
        protected override void ProcessCmdletRecord()
        {
            var request = new RestRequest($"wit/workitems/{this.Id}");

            //// request.RequestFormat = DataFormat.Json;

            if (this.OriginalWorkItem == null)
            {
                var getRequest = new RestRequest($"wit/workitems/{this.Id}");
                var getResponse = this.Client.Execute<WorkItem>(getRequest);

                if (getResponse.IsSuccessful)
                {
                    this.OriginalWorkItem = getResponse.Data;
                }
                else
                {
                    this.ProcessErrorResponse(
                        getResponse,
                        DevOpsModelTarget.WorkItem,
                        ErrorCategory.NotSpecified,
                        this);
                }
            }

            var patchDocument = JsonHelpers.CreatePatch(this.OriginalWorkItem, this.UpdatedWorkItem);

            request.AddJsonBody(patchDocument);
            request.AddParameter("Content-Type", "application/json-patch+json", ParameterType.HttpHeader);
            var restResponse = this.Client.Patch(request);
            var updateWorkItem = JsonConvert.DeserializeObject<WorkItem>(restResponse.Content);

            if (this.IsDebug)
            {
                // ReSharper disable once ExceptionNotDocumented
                this.SetPsVariable("UpdateRestResponse", restResponse);
            }

            if (!restResponse.IsSuccessful)
            {
                this.ProcessErrorResponse(restResponse, DevOpsModelTarget.WorkItem, ErrorCategory.NotSpecified, this);
            }
            else if (this.UpdatedWorkItem.Rev == updateWorkItem.Rev)
            {
                this.ThrowTerminatingError(new ErrorRecord(new PatchOperationFailedException("The update was not applied the selected work item"), "AzureDevOpsMgmt.Core.Cmdlet.UpdateWorkItem.RevisionNumberIsEqual", ErrorCategory.InvalidResult, request));
            }

            this.WriteObject("The operation has completed successfully");
        }
    }
}
