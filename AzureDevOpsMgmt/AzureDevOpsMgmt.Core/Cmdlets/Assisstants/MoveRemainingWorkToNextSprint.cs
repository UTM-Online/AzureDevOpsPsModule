// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 06-12-2019
// ***********************************************************************
// <copyright file="MoveRemainingWorkToNextSprint.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Cmdlets.Assisstants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using Helpers;
    using Microsoft.TeamFoundation.Work.WebApi;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Models;
    using Resources;
    using RestSharp;

    /// <summary>
    /// Class MoveRemainingWorkToNextSprint.
    /// Implements the <see cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    /// </summary>
    /// <seealso cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    public class MoveRemainingWorkToNextSprint : ApiCmdlet
    {
        /// <summary>
        /// Gets or sets the move option.
        /// </summary>
        /// <value>The move option.</value>
        [Parameter(Mandatory = true)]
        public MoveOption MoveOption { get; set; }

        /// <summary>
        /// Gets or sets the source sprint.
        /// </summary>
        /// <value>The source sprint.</value>
        [Parameter]
        public string SourceSprint { get; set; }

        /// <summary>
        /// Gets or sets the destination sprint.
        /// </summary>
        /// <value>The destination sprint.</value>
        [Parameter]
        public string DestinationSprint { get; set; }

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        [ShouldInject("AdoTeamsApi")]
        protected override IRestClient Client { get; set; }

        /// <summary>
        /// Gets or sets the known iterations.
        /// </summary>
        /// <value>The known iterations.</value>
        private IEnumerable<TeamSettingsIteration> KnownIterations { get; set; }

        /// <summary>
        /// Begins the processing cmdlet.
        /// </summary>
        protected override void BeginProcessingCmdlet()
        {
            if (string.IsNullOrWhiteSpace(this.SourceSprint) || string.IsNullOrWhiteSpace(this.DestinationSprint))
            {
                var knownSprintsRequest = new RestRequest("/work/teamsettings/iterations");

                var knownSprintsResponse = this.Client.Get<List<TeamSettingsIteration>>(knownSprintsRequest);

                if (knownSprintsResponse.IsSuccessful)
                {
                    this.KnownIterations = knownSprintsResponse.Data;
                }
                else
                {
                    this.WriteError(knownSprintsResponse.ErrorException, this.BuildStandardErrorId(DevOpsModelTarget.AreasAndIterations), ErrorCategory.NotSpecified, knownSprintsResponse);
                }
            }

            this.ProcessSourceSprint();
            this.ProcessDestinationSprint();
        }

        /// <summary>
        /// Processes the cmdlet record.
        /// </summary>
        /// <inheritdoc />
        protected override void ProcessCmdletRecord()
        {
            var queryParams = this.CreateParamDictionary().AddParam(
                                                                    "Query",
                                                                    string.Format(WorkItemQueries.AllActiveBugsAndTasksForIteration, this.SourceSprint));

            var queryResult = this.InvokeModuleCmdlet<WorkItem>("Search-WorkItem", queryParams);

            foreach (var item in queryResult)
            {
                var updatedWorkItem = item.DeepCopy();
                updatedWorkItem.Fields["System.IterationPath"] = this.DestinationSprint;

                var updateCmdletParams = this.CreateParamDictionary().AddParam("Id", item.Id)
                                             .AddParam("UpdatedWorkItem", updatedWorkItem)
                                             .AddParam("OriginalWorkItem", item);

                this.InvokeModuleCmdlet("Update-WorkItem", updateCmdletParams);
            }
        }

        /// <summary>
        /// Processes the source sprint.
        /// </summary>
        private void ProcessSourceSprint()
        {
            if (string.IsNullOrWhiteSpace(this.SourceSprint))
            {
                var discoveredSourceSprint =
                    this.InvokeModuleCmdlet<TeamSettingsIteration>("Get-CurrentIteration").First();

                this.SourceSprint = $"{discoveredSourceSprint.Path}{discoveredSourceSprint.Name}";
            }
            else
            {
                var parsedSourceSprint = this.ParseSprintString(this.SourceSprint);

                if (!this.KnownIterations.Any(s => s.Name == parsedSourceSprint.SprintName && s.Path == parsedSourceSprint.SprintPath))
                {
                    // ReSharper disable once StyleCop.SA1116
                    // ReSharper disable once LocalizableElement
                    this.WriteError(
                                    new ArgumentException("Source Sprint Not Found.", nameof(this.SourceSprint)),
                                    // ReSharper disable once StyleCop.SA1118
                                    this.BuildStandardErrorId(
                                                              DevOpsModelTarget.WorkItem,
                                                              "Specified Source Sprint Not Found In Azure Dev Ops"),
                                    ErrorCategory.InvalidArgument,
                                    this.SourceSprint);
                }
            }
        }

        /// <summary>
        /// Processes the destination sprint.
        /// </summary>
        private void ProcessDestinationSprint()
        {
            if (string.IsNullOrWhiteSpace(this.DestinationSprint))
            {
                var discoveredDestinationSprint = this.InvokeModuleCmdlet<TeamSettingsIteration>("Get-NextIteration").First();

                this.DestinationSprint = $"{discoveredDestinationSprint.Path}{discoveredDestinationSprint.Name}";
            }
            else
            {
                var parsedDestinationSprint = this.ParseSprintString(this.DestinationSprint);

                if (!this.KnownIterations.Any(d => d.Name == parsedDestinationSprint.SprintName && d.Path == parsedDestinationSprint.SprintPath))
                {
                    // ReSharper disable once LocalizableElement
                    this.WriteError(new ArgumentException("Destination Sprint Not Found.", nameof(this.DestinationSprint)), this.BuildStandardErrorId(DevOpsModelTarget.AreasAndIterations), ErrorCategory.NotSpecified, this.DestinationSprint);
                }
            }
        }

        /// <summary>
        /// Parses the sprint string.
        /// </summary>
        /// <param name="sprint">The sprint.</param>
        /// <returns>a dynamic object.</returns>
        private dynamic ParseSprintString(string sprint)
        {
            var nameIndex = sprint.LastIndexOf('/');

            return new
            {
                SprintName = sprint.Substring(nameIndex + 1, this.SourceSprint.Length),
                SprintPath = sprint.Substring(0, nameIndex)
            };
        }
    }
}
