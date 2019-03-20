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

        /// <summary>
        /// The timing data
        /// </summary>
        private dynamic timingData = new { };

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
        protected override void BeginProcessing()
        {
            this.originalWorkItem = this.InvokePsCommand<WorkItem>("AzureDevOpsMgmt\\Get-WorkItem", new Dictionary<string, object> { { "Id", this.Id } }).First();

            this.originalWorkItem.Fields.TryGetValue("Microsoft.VSTS.Scheduling.RemainingWork", out this.timingData.RemainingWork);
            this.originalWorkItem.Fields.TryGetValue("Microsoft.VSTS.Scheduling.OriginalEstimate", out this.timingData.OriginalEstimate);
            this.originalWorkItem.Fields.TryGetValue("Microsoft.VSTS.Scheduling.CompletedWork", out this.timingData.CompletedWork);
        }

        /// <summary>
        /// When overridden in the derived class, performs clean-up
        /// after the command execution.
        /// Default implementation in the base class just returns.
        /// </summary>
        protected override void EndProcessing()
        {
            var invocationDictionary = new Dictionary<string, object> { { "Id", this.Id }, { "UpdatedWorkItem", this.originalWorkItem } };

            this.InvokePsCommand("AzureDevOps\\Update-WorkItem", invocationDictionary);
        }

        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        /// <exception cref="T:System.OverflowException">Represents a number that is less than <see cref="F:System.TimeSpan.MinValue" /> or greater than <see cref="F:System.TimeSpan.MaxValue" />.-or- At least one of the days, hours, minutes, or seconds components is outside its valid range.</exception>
        /// <exception cref="T:System.ArgumentNullException">Argument is <see langword="null" />.</exception>
        protected override void ProcessRecord()
        {
            var completedWork = new TimeSpan();

            foreach (var ts in this.WorkCompletedThisSession)
            {
                completedWork += TimeSpan.Parse(ts);
            }

            TimeSpan remainingWorkTs = TimeSpan.FromHours(this.timingData.RemainingWork);

            var newRemainingWork = (remainingWorkTs - completedWork).TotalHours;
            double newCompletedWorkHours;

            try
            {
                TimeSpan existingCompletedWorkHoursTs = TimeSpan.FromHours(this.timingData.CompletedWork);
                newCompletedWorkHours = (completedWork + existingCompletedWorkHoursTs).TotalHours;
                newCompletedWorkHours = Math.Round(newCompletedWorkHours, 4, MidpointRounding.ToEven);
            }
            catch (Exception)
            {
                newCompletedWorkHours = completedWork.TotalHours;
            }

            this.originalWorkItem.Fields["Microsoft.VSTS.Scheduling.RemainingWork"] = newRemainingWork;
            this.originalWorkItem.Fields["Microsoft.VSTS.Scheduling.CompletedWork"] = newCompletedWorkHours;

            if (!string.IsNullOrWhiteSpace(this.Description))
            {
                this.originalWorkItem.Fields.Add("System.History", this.Description);
            }
        }
    }
}
