// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 06-26-2019
// ***********************************************************************
// <copyright file="DenyReleaseStep.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Cmdlets.ReleaseManagement
{
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;

    using RestSharp;

    /// <summary>
    /// Class DenyReleaseStep.
    /// Implements the <see cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    /// </summary>
    /// <seealso cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    [Cmdlet(VerbsLifecycle.Deny, "ReleaseStep")]
    public class DenyReleaseStep : ApiCmdlet
    {
        /// <summary>
        /// Gets or sets the approval identifier.
        /// </summary>
        /// <value>The approval identifier.</value>
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [Alias("id")]
        public int ApprovalId { get; set; }

        /// <summary>
        /// Gets or sets the reason.
        /// </summary>
        /// <value>The reason.</value>
        [Parameter(Mandatory = true)]
        public string Reason { get; set; }

        /// <summary>
        /// Processes the cmdlet record.
        /// </summary>
        protected override void ProcessCmdletRecord()
        {
            var request = new RestRequest($"release/approvals/{this.ApprovalId}");

            var requestBody = new { status = "rejected", comments = this.Reason };

            request.AddJsonBody(requestBody);

            var response = this.Client.Patch<ReleaseApproval>(request);

            this.WriteObject(response, DevOpsModelTarget.Release, ErrorCategory.NotSpecified, this);
        }
    }
}
