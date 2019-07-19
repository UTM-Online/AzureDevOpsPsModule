// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : josh
// Created          : 07-16-2019
//
// Last Modified By : josh
// Last Modified On : 07-16-2019
// ***********************************************************************
// <copyright file="GetCurrentIteration.cs" company="UTM-Online">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AzureDevOpsMgmt.Cmdlets.WorkItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;

    using Microsoft.TeamFoundation.Work.WebApi;

    using RestSharp;

    /// <summary>
    ///     Class GetCurrentIteration.
    ///     Implements the <see cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    /// </summary>
    /// <seealso cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    public class GetCurrentIteration : ApiCmdlet
    {
        /// <summary>
        ///     Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        [ShouldInject("AdoTeamsApi")]
        protected override IRestClient Client { get; set; }

        /// <summary>
        ///     Processes the cmdlet record.
        /// </summary>
        protected override void ProcessCmdletRecord()
        {
            var request = new RestRequest("/work/teamsettings/iterations", Method.GET);
            request.AddQueryParameter("$timeframe", "current");

            var response = this.Client.Execute<List<TeamSettingsIteration>>(request);

            if (!response.IsSuccessful)
            {
                this.ProcessErrorResponse(
                    response,
                    DevOpsModelTarget.AreasAndIterations,
                    ErrorCategory.NotSpecified,
                    this);
            }

            if (!response.Data.Any())
            {
                this.WriteError(
                    new Exception("No Iterations Found!"),
                    this.BuildStandardErrorId(DevOpsModelTarget.AreasAndIterations),
                    ErrorCategory.NotSpecified,
                    response.Data);
            }

            this.WriteObject(response.Data.First());
        }
    }
}