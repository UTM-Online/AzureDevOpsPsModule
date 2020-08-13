// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="GetNextIteration.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Cmdlets.WorkItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using Helpers;
    using Microsoft.TeamFoundation.Work.WebApi;
    using Models;
    using RestSharp;
    using UTMO.Common.Guards;

    /// <summary>
    /// Class GetNextIteration.
    /// Implements the <see cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    /// </summary>
    /// <seealso cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    [Cmdlet(VerbsCommon.Get, "NextIteration")]
    public class GetNextIteration : ApiCmdlet
    {
        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        /// <inheritdoc />
        [ShouldInject("AdoTeamsApi")]
        protected override IRestClient Client { get; set; }

        /// <summary>
        /// Begins the processing cmdlet.
        /// </summary>
        /// <inheritdoc />
        protected override void BeginProcessingCmdlet()
        {
            Guard.Requires<InvalidOperationException>(!string.IsNullOrWhiteSpace(AzureDevOpsConfiguration.Config.CurrentConnection.CurrentTeam), "Use of this cmdlet requires a team be specified.  Please use the \"Set-AzureDevOpsProjectTeam\" cmdlet first then rerun this cmdlet.");
        }

        /// <summary>
        /// Processes the cmdlet record.
        /// </summary>
        /// <inheritdoc />
        protected override void ProcessCmdletRecord()
        {
            var request = new RestRequest("/work/teamsettings/iterations", Method.GET);
            request.AddQueryParameter("$timeframe", "future");

            var response = this.Client.Execute<List<TeamSettingsIteration>>(request);

            if (response.IsSuccessful && response.Data.Any())
            {
                this.WriteObject(response.Data.First());
            }
            else
            {
                this.WriteError(response.ErrorException, this.BuildStandardErrorId(DevOpsModelTarget.WorkItem), ErrorCategory.NotSpecified, response);
            }
        }
    }
}