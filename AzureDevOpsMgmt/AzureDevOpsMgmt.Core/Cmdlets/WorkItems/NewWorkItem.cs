// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 08-30-2019
// ***********************************************************************
// <copyright file="NewWorkItem.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Cmdlets.WorkItems
{
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    using Helpers;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

    using RestSharp;

    /// <summary>
    /// Class NewWorkItem.
    /// Implements the <see cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    /// </summary>
    /// <seealso cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    public class NewWorkItem : ApiCmdlet
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [ValidateSet("Task", "Bug")]
        [Parameter(Mandatory = true, ParameterSetName = "Main")]
        [Parameter(Mandatory = true, ParameterSetName = "NoEstimation")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Parameter(Mandatory = true, ParameterSetName = "Main")]
        [Parameter(Mandatory = true, ParameterSetName = "NoEstimation")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the area path.
        /// </summary>
        /// <value>The area path.</value>
        [Parameter(Mandatory = true, ParameterSetName = "Main")]
        [Parameter(Mandatory = true, ParameterSetName = "NoEstimation")]
        public string AreaPath { get; set; }

        /// <summary>
        /// Gets or sets the iteration path.
        /// </summary>
        /// <value>The iteration path.</value>
        [Parameter(Mandatory = true, ParameterSetName = "Main")]
        [Parameter(Mandatory = true, ParameterSetName = "NoEstimation")]
        public string IterationPath { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>The parent identifier.</value>
        [Parameter(ParameterSetName = "Main")]
        [Parameter(ParameterSetName = "NoEstimation")]
        public long? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the best case estimate.
        /// </summary>
        /// <value>The best case estimate.</value>
        [Parameter(Mandatory = true, ParameterSetName = "Main")]
        public int? BestCaseEstimate { get; set; }

        /// <summary>
        /// Gets or sets the most likely case estimate.
        /// </summary>
        /// <value>The most likely case estimate.</value>
        [Parameter(Mandatory = true, ParameterSetName = "Main")]
        public int? MostLikelyCaseEstimate { get; set; }

        /// <summary>
        /// Gets or sets the worst case estimate.
        /// </summary>
        /// <value>The worst case estimate.</value>
        [Parameter(Mandatory = true, ParameterSetName = "Main")]
        public int? WorstCaseEstimate { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [Parameter(ParameterSetName = "Main")]
        [Parameter(ParameterSetName = "NoEstimation")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        [Parameter(ParameterSetName = "Main")]
        [Parameter(ParameterSetName = "NoEstimation")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Gets or sets the triage approved.
        /// </summary>
        /// <value>The triage approved.</value>
        [Parameter(ParameterSetName = "Main")]
        [Parameter(ParameterSetName = "NoEstimation")]
        public SwitchParameter TriageApproved { get; set; }

        /// <summary>
        /// Gets or sets the open on completion.
        /// </summary>
        /// <value>The open on completion.</value>
        [Parameter(ParameterSetName = "Main")]
        [Parameter(ParameterSetName = "NoEstimation")]
        public SwitchParameter OpenOnCompletion { get; set; }

        /// <summary>
        /// Gets or sets the skip self assignment.
        /// </summary>
        /// <value>The skip self assignment.</value>
        [Parameter(ParameterSetName = "Main")]
        [Parameter(ParameterSetName = "NoEstimation")]
        public SwitchParameter SkipSelfAssignment { get; set; }

        /// <summary>
        /// Gets or sets the skip task estimation.
        /// </summary>
        /// <value>The skip task estimation.</value>
        [Parameter(Mandatory = true, ParameterSetName = "NoEstimation")]
        public SwitchParameter SkipTaskEstimation { get; set; }

        /// <summary>
        /// Gets the patch document.
        /// </summary>
        /// <value>The patch document.</value>
        private JsonPatchDocument PatchDocument { get; } = new JsonPatchDocument();

        /// <summary>
        /// Gets or sets the created work item.
        /// </summary>
        /// <value>The created work item.</value>
        private WorkItem CreatedWorkItem { get; set; }

        /// <summary>
        /// Begins the processing cmdlet.
        /// </summary>
        /// <inheritdoc />
        protected override void BeginProcessingCmdlet()
        {
            this.PatchDocument.Add("/fields/System.Title", this.Name)
                .Add("/fields/System.AreaPath", this.AreaPath)
                .Add("/fields/System.IterationPath", this.IterationPath)
                .AddIfNotNull("/fields/System.Description", this.Description)
                .AddIfNotNull("/fields/System.Tags", string.Join(";", this.Tags));

            if (!this.SkipTaskEstimation)
            {
                var originalEstimation = (this.BestCaseEstimate + (this.MostLikelyCaseEstimate * 4) + this.WorstCaseEstimate) / 6;

                this.PatchDocument.Add("/fields/Microsoft.VSTS.Scheduling.OriginalEstimate", originalEstimation)
                    .Add("/fields/Microsoft.VSTS.Scheduling.RemainingWork", originalEstimation)
                    .Add("/fields/Microsoft.VSTS.Scheduling.CompletedWork", 0);
            }

            if (this.TriageApproved)
            {
                this.PatchDocument.Add("/fields/Microsoft.VSTS.Common.Triage", "Approved");
            }
        }

        /// <summary>
        /// Processes the cmdlet record.
        /// </summary>
        /// <inheritdoc />
        protected override void ProcessCmdletRecord()
        {
            var requestType = this.Type == "Task" ? "Task" : "Bug";

            var createRequest = new RestRequest($"/wit/workitems/${requestType}", Method.POST);

            createRequest.AddJsonBody(this.PatchDocument);

            var createResponse = this.Client.Execute<WorkItem>(createRequest);

            if (!createResponse.IsSuccessful)
            {
                this.WriteError(createResponse.ErrorException, this.BuildStandardErrorId(DevOpsModelTarget.WorkItem), ErrorCategory.NotSpecified, createRequest);
            }
            else if (!this.SkipSelfAssignment || this.ParentId != null)
            {
                var newWorkItem = createResponse.Data.DeepCopy();

                if (!this.SkipSelfAssignment)
                {
                    var createdBy = newWorkItem.Fields["System.CreatedBy"].ToString();

                    newWorkItem.Fields["System.AssignedTo"] = createdBy; 
                }

                if (this.ParentId != null)
                {
                    var parentWorkItem = this.GetWorkItem((long)this.ParentId);

                    newWorkItem.Relations.Add(new WorkItemRelation()
                                                  {
                                                      Rel = "System.LinkTypes.Hierarchy-Reverse",
                                                      Url = parentWorkItem.Url
                                                  });
                }

                this.CreatedWorkItem = this.UpdateWorkItem(createResponse.Data, newWorkItem);
            }
            else
            {
                this.CreatedWorkItem = createResponse.Data;
            }
        }

        /// <summary>
        /// Ends the cmdlet processing.
        /// </summary>
        /// <inheritdoc />
        protected override void EndCmdletProcessing()
        {
            this.WriteObject(this.CreatedWorkItem);

            if (this.OpenOnCompletion)
            {
                System.Diagnostics.Process.Start(this.CreatedWorkItem.Links.Links["html"].ToString());
            }
        }
    }
}