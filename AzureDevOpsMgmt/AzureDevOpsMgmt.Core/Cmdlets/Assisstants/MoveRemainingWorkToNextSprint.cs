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
    using System.Management.Automation;
    using Helpers;
    using Microsoft.TeamFoundation.Work.WebApi;
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
        /// Gets the override API path.
        /// </summary>
        /// <value>The override API path.</value>
        protected override string OverrideApiPath => $"/{Uri.EscapeUriString(AzureDevOpsConfiguration.Config.CurrentConnection.ProjectName)}/_apis";

        /// <summary>
        /// Begins the processing cmdlet.
        /// </summary>
        protected override void BeginProcessingCmdlet()
        {
            var sourceSprintVerificationRequest = new RestRequest("/work/teamsettings/iterations");

            var sourceSprintVerificationResponse = this.Client.Get<List<TeamSettingsIteration>>(sourceSprintVerificationRequest);

            if (sourceSprintVerificationResponse.IsSuccessful)
                // ReSharper disable once StyleCop.SA1505
            {
            }
            else
            {
                this.WriteError(sourceSprintVerificationResponse.ErrorException, this.BuildStandardErrorId(DevOpsModelTarget.WorkItem), ErrorCategory.InvalidResult, this);
            }
        }
    }
}
