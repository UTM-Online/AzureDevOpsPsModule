// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : joirwi
// Created          : 03-20-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-20-2019
// ***********************************************************************
// <copyright file="UpdateRemainingWork.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AzureDevOpsMgmt.Cmdlets.Assisstants
{
    using System;
    using System.Collections.Generic;
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
    public class UpdateRemainingWork : PSCmdlet
    {
        /// <summary>
        /// The original work item
        /// </summary>
        private WorkItem originalWorkItem;

        /// <summary>The update work item</summary>
        private WorkItem updateWorkItem;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [Parameter(Position = 3)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Parameter(Mandatory = true, Position = 1)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the work completed this session.
        /// </summary>
        /// <value>The work completed this session.</value>
        [Parameter(Mandatory = true, Position = 2)]
        public string[] WorkCompletedThisSession { get; set; }

        /// <summary>
        /// When overridden in the derived class, performs initialization
        /// of command execution.
        /// Default implementation in the base class just returns.
        /// </summary>
        /// <exception cref="T:System.Management.Automation.PipelineStoppedException">The pipeline has already been terminated, or was terminated
        ///             during the execution of this method.
        ///             The Cmdlet should generally just allow PipelineStoppedException
        ///             to percolate up to the caller of ProcessRecord etc.
        /// </exception>
        protected override void BeginProcessing()
        {
            this.WriteDebug($"Loading data for work item {this.Id}");
            this.originalWorkItem = this.InvokeModuleCmdlet<WorkItem>("Get-WorkItem", new Dictionary<string, object> { { "Id", this.Id } }).First();
            this.updateWorkItem = this.originalWorkItem.DeepCopy();
            this.WriteDebug($"Data loaded for work item {this.originalWorkItem.Fields["System.Title"]} have been loaded and cloned");
        }

        /// <summary>
        /// When overridden in the derived class, performs clean-up
        /// after the command execution.
        /// Default implementation in the base class just returns.
        /// </summary>
        /// <exception cref="T:System.Management.Automation.PipelineStoppedException">The pipeline has already been terminated, or was terminated
        ///             during the execution of this method.
        ///             The Cmdlet should generally just allow PipelineStoppedException
        ///             to percolate up to the caller of ProcessRecord etc.
        /// </exception>
        /// <exception cref="T:System.Management.Automation.SessionStateOverflowException">If the maximum number of variables has been reached for this scope.</exception>
        /// <exception cref="T:System.Management.Automation.ProviderNotFoundException">If the variable refers to a provider that could not be found.</exception>
        /// <exception cref="T:System.Management.Automation.DriveNotFoundException">If the variable refers to a drive that could not be found.</exception>
        /// <exception cref="T:System.Management.Automation.ProviderInvocationException">If the provider threw an exception.</exception>
        /// <exception cref="T:System.Management.Automation.SessionStateUnauthorizedAccessException">If the variable is read-only or constant.</exception>
        protected override void EndProcessing()
        {
            this.WriteDebug("Creating command invocation dictionary");
            var invocationDictionary =
                new Dictionary<string, object> { { "Id", this.Id }, { "UpdatedWorkItem", this.updateWorkItem }, { "OriginalWorkItem", this.originalWorkItem } };

            if (this.MyInvocation.BoundParameters.ContainsKey("Debug"))
            {
                invocationDictionary.Add("Debug", true);
            }

            this.WriteDebug("Sending patch file to Azure DevOps Rest API");
            this.InvokeModuleCmdlet("Update-WorkItem", invocationDictionary);

            if (this.MyInvocation.BoundParameters.ContainsKey("Debug"))
            {
                this.WriteDebug("Setting output Variables with work item data");
                this.SetPsVariable("OriginalWorkItem", this.originalWorkItem);
                this.SetPsVariable("UpdatedWorkItem", this.updateWorkItem);
            }

            this.WriteDebug("Invocation of Update-RemainingWork has completed.");
        }

        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        /// <exception cref="T:System.OverflowException">Represents a number that is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.-or- At least one of the days, hours, minutes, or seconds components is outside its valid range.</exception>
        /// <exception cref="T:System.ArgumentNullException">Argument is <see langword="null" />.</exception>
        /// <exception cref="T:System.Management.Automation.PipelineStoppedException">The pipeline has already been terminated, or was terminated
        ///             during the execution of this method.
        ///             The Cmdlet should generally just allow PipelineStoppedException
        ///             to percolate up to the caller of ProcessRecord etc.
        /// </exception>
        protected override void ProcessRecord()
        {
            this.WriteDebug("Loading required field values from work item");
            this.originalWorkItem.Fields.TryGetValue("Microsoft.VSTS.Scheduling.RemainingWork", out var rawRemainingWork);

            this.WriteDebug($"Remaining Work field value is {rawRemainingWork ?? "Unspecified"}");
            this.originalWorkItem.Fields.TryGetValue("Microsoft.VSTS.Scheduling.OriginalEstimate", out var rawOriginalEstimate);

            this.WriteDebug($"Original Estimate field value is {rawOriginalEstimate ?? "Unspecified"}");
            this.originalWorkItem.Fields.TryGetValue("Microsoft.VSTS.Scheduling.CompletedWork", out var rawCompletedWork);

            this.WriteDebug($"Completed Work field value is {rawCompletedWork ?? "Unspecified"}");
            this.WriteDebug("Beginning Work Time Calculations");

            var completedWork = new TimeSpan();

            foreach (var ts in this.WorkCompletedThisSession)
            {
                completedWork += TimeSpan.Parse(ts);
            }

            var remainingWorkTs = new TimeSpan();

            if (rawRemainingWork != null)
            {
                remainingWorkTs = TimeSpan.FromHours((double)rawRemainingWork);
            }
            else if (rawOriginalEstimate != null)
            {
                remainingWorkTs = TimeSpan.FromHours((double)rawOriginalEstimate);
            }

            double newRemainingWork;

            if (remainingWorkTs.TotalHours <= 0)
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
            this.WriteDebug($"The new remaining work value is {this.updateWorkItem.Fields["Microsoft.VSTS.Scheduling.RemainingWork"] ?? "N/A"}");
            this.updateWorkItem.Fields["Microsoft.VSTS.Scheduling.CompletedWork"] = newCompletedWorkHours;
            this.WriteDebug($"The new compleated work value is {this.updateWorkItem.Fields["Microsoft.VSTS.Scheduling.CompletedWork"] ?? "N/A"}");

            if (!string.IsNullOrWhiteSpace(this.Description))
            {
                this.updateWorkItem.Fields.Add("System.History", this.Description);
                this.WriteDebug($"The description to be appended to the update is {this.updateWorkItem.Fields["System.History"] ?? "ERROR"}");
            }
        }
    }
}
