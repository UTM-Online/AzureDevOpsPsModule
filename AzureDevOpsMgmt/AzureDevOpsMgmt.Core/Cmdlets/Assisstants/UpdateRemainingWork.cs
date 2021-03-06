﻿// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="UpdateRemainingWork.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************

namespace AzureDevOpsMgmt.Cmdlets.Assisstants
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Helpers;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    /// <summary>
    /// Class UpdateRemainingWork.
    /// Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    [Cmdlet(VerbsData.Update, "RemainingWork")]
    [SuppressMessage(
        "ReSharper",
        "MemberCanBePrivate.Global",
        Justification = "Public properties must be public to be visible in shell")]
    public class UpdateRemainingWork : PSCmdlet
    {
        /// <summary>
        /// The original work item
        /// </summary>
        private WorkItem originalWorkItem;

        /// <summary>
        /// The update work item
        /// </summary>
        private WorkItem updateWorkItem;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [Parameter]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Parameter(Mandatory = true)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the work completed this session.
        /// </summary>
        /// <value>The work completed this session.</value>
        [Parameter(Mandatory = true)]
        public string[] WorkCompletedThisSession { get; set; }

        /// <summary>
        /// When overridden in the derived class, performs initialization
        /// of command execution.
        /// Default implementation in the base class just returns.
        /// </summary>
        /// <exception cref="T:System.Management.Automation.PipelineStoppedException">The pipeline has already been terminated, or was terminated
        /// during the execution of this method.
        /// The Cmdlet should generally just allow PipelineStoppedException
        /// to percolate up to the caller of ProcessRecord etc.</exception>
        protected override void BeginProcessing()
        {
            this.WriteVerbose($"Loading data for work item {this.Id}");
            this.originalWorkItem = this.InvokeModuleCmdlet<WorkItem>(
                "Get-WorkItem",
                new Dictionary<string, object> { { "Id", this.Id } }).First();
            this.updateWorkItem = this.originalWorkItem.DeepCopy();
            this.WriteVerbose(
                $"Data loaded for work item \"{this.originalWorkItem.Fields["System.Title"]}\" have been loaded and cloned");
        }

        /// <summary>
        /// When overridden in the derived class, performs clean-up
        /// after the command execution.
        /// Default implementation in the base class just returns.
        /// </summary>
        /// <exception cref="T:System.Management.Automation.PipelineStoppedException">The pipeline has already been terminated, or was terminated
        /// during the execution of this method.
        /// The Cmdlet should generally just allow PipelineStoppedException
        /// to percolate up to the caller of ProcessRecord etc.</exception>
        /// <exception cref="T:System.Management.Automation.SessionStateOverflowException">If the maximum number of variables has
        /// been reached for this scope.</exception>
        /// <exception cref="T:System.Management.Automation.ProviderNotFoundException">If the variable refers to a provider that
        /// could not be found.</exception>
        /// <exception cref="T:System.Management.Automation.DriveNotFoundException">If the variable refers to a drive that could
        /// not be found.</exception>
        /// <exception cref="T:System.Management.Automation.ProviderInvocationException">If the provider threw an exception.</exception>
        /// <exception cref="T:System.Management.Automation.SessionStateUnauthorizedAccessException">If the variable is read-only
        /// or constant.</exception>
        protected override void EndProcessing()
        {
            this.WriteVerbose("Creating command invocation dictionary");
            var invocationDictionary = new Dictionary<string, object>
                                           {
                                               { "Id", this.Id },
                                               { "UpdatedWorkItem", this.updateWorkItem },
                                               { "OriginalWorkItem", this.originalWorkItem }
                                           };

            if (this.MyInvocation.BoundParameters.ContainsKey("Debug"))
            {
                invocationDictionary.Add("Debug", true);
            }

            this.WriteVerbose("Sending patch file to Azure DevOps Rest API");
            this.InvokeModuleCmdlet("Update-WorkItem", invocationDictionary);

            if (this.MyInvocation.BoundParameters.ContainsKey("Debug"))
            {
                this.WriteVerbose("Setting output Variables with work item data");
                this.SetPsVariable("OriginalWorkItem", this.originalWorkItem);
                this.SetPsVariable("UpdatedWorkItem", this.updateWorkItem);
            }

            this.WriteVerbose("Invocation of Update-RemainingWork has completed.");
        }

        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        /// <exception cref="T:System.OverflowException">Represents a number that is less than
        /// <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.-or- At least
        /// one of the days, hours, minutes, or seconds components is outside its valid range.</exception>
        /// <exception cref="T:System.ArgumentNullException">Argument is <see langword="null" />.</exception>
        /// <exception cref="T:System.Management.Automation.PipelineStoppedException">The pipeline has already been terminated, or was terminated
        /// during the execution of this method.
        /// The Cmdlet should generally just allow PipelineStoppedException
        /// to percolate up to the caller of ProcessRecord etc.</exception>
        [SuppressMessage("ReSharper", "StyleCop.SA1126", Justification = "Suppression Approved!")]
        protected override void ProcessRecord()
        {
            this.WriteVerbose("Loading required field values from work item");
            this.originalWorkItem.Fields.TryGetValue(
                "Microsoft.VSTS.Scheduling.RemainingWork",
                out var rawRemainingWork);

            this.WriteVerbose($"Remaining Work field value is {rawRemainingWork ?? "Unspecified"}");
            this.originalWorkItem.Fields.TryGetValue(
                "Microsoft.VSTS.Scheduling.OriginalEstimate",
                out var rawOriginalEstimate);

            this.WriteVerbose($"Original Estimate field value is {rawOriginalEstimate ?? "Unspecified"}");
            this.originalWorkItem.Fields.TryGetValue(
                "Microsoft.VSTS.Scheduling.CompletedWork",
                out var rawCompletedWork);

            this.WriteVerbose($"Completed Work field value is {rawCompletedWork ?? "Unspecified"}");
            this.WriteVerbose("Beginning Work Time Calculations");

            var completedWork = new TimeSpan();

            foreach (var ts in this.WorkCompletedThisSession)
            {
                completedWork += TimeSpan.Parse(ts);
            }

            var remainingWorkTs = new TimeSpan();

            if (rawRemainingWork != null)
            {
                remainingWorkTs = TimeSpan.FromHours((double)(rawRemainingWork ?? 0));
            }
            else if (rawOriginalEstimate != null)
            {
                remainingWorkTs = TimeSpan.FromHours((double)(rawOriginalEstimate ?? 0));
            }

            double newRemainingWork;

            if (remainingWorkTs.TotalHours <= completedWork.TotalHours)
            {
                newRemainingWork = 0;
            }
            else
            {
                newRemainingWork = Math.Round((remainingWorkTs - completedWork).TotalHours, 4, MidpointRounding.ToEven);
            }

            double newCompletedWorkHours;

            if (rawCompletedWork != null)
            {
                var existingCompletedWorkHoursTs = TimeSpan.FromHours((double)rawCompletedWork);
                newCompletedWorkHours = (completedWork + existingCompletedWorkHoursTs).TotalHours;
                newCompletedWorkHours = Math.Round(newCompletedWorkHours, 4, MidpointRounding.ToEven);
            }
            else
            {
                newCompletedWorkHours = completedWork.TotalHours;
            }

            this.updateWorkItem.Fields["Microsoft.VSTS.Scheduling.RemainingWork"] = newRemainingWork;
            this.WriteVerbose(
                $"The new remaining work value is {this.updateWorkItem.Fields["Microsoft.VSTS.Scheduling.RemainingWork"] ?? "N/A"}");
            this.updateWorkItem.Fields["Microsoft.VSTS.Scheduling.CompletedWork"] = newCompletedWorkHours;
            this.WriteVerbose(
                $"The new compleated work value is {this.updateWorkItem.Fields["Microsoft.VSTS.Scheduling.CompletedWork"] ?? "N/A"}");

            if (!string.IsNullOrWhiteSpace(this.Description))
            {
                this.updateWorkItem.Fields.Add("System.History", this.Description);
                this.WriteVerbose(
                    $"The description to be appended to the update is {this.updateWorkItem.Fields["System.History"] ?? "ERROR"}");
            }
        }
    }
}