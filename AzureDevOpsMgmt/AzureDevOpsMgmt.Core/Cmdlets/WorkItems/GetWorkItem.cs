﻿// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="GetWorkItem.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************

namespace AzureDevOpsMgmt.Cmdlets.WorkItems
{
    using System.Diagnostics.CodeAnalysis;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    using RestSharp;

    /// <summary>
    /// Class GetWorkItem.
    /// Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// Implements the <see cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    /// </summary>
    /// <seealso cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    [Cmdlet(VerbsCommon.Get, "WorkItem")]
    public class GetWorkItem : ApiCmdlet
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Parameter]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>The fields.</value>
        [Parameter]
        public string Fields { get; set; }

        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        protected override void ProcessCmdletRecord()
        {
            var workItemRequest = new RestRequest($"wit/workitems/{this.Id}", Method.GET);

            if (!string.IsNullOrWhiteSpace(this.Fields))
            {
                workItemRequest.AddQueryParameter("fields", this.Fields);
            }

            var workItemResponse = this.Client.Execute<WorkItem>(workItemRequest);

            this.WriteObject(workItemResponse, DevOpsModelTarget.WorkItem, ErrorCategory.NotSpecified, this);
        }
    }
}
