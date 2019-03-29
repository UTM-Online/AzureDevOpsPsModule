﻿namespace AzureDevOpsMgmt.Cmdlets.ReleaseManagement
{
    using System.Management.Automation;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;

    using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;

    using RestSharp;

    [Cmdlet(VerbsLifecycle.Deny, "ReleaseStep")]
    public class DenyReleaseStep : ApiCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [Alias("id")]
        public int ApprovalId { get; set; }

        [Parameter(Mandatory = true)]
        public string Reason { get; set; }

        protected override void ProcessRecord()
        {
            var request = new RestRequest($"release/approvals/{this.ApprovalId}");

            var requestBody = new { status = "rejected", comments = this.Reason };

            request.AddJsonBody(requestBody);

            var response = this.client.Patch<ReleaseApproval>(request);

            this.WriteObject(response, DevOpsModelTarget.Release, ErrorCategory.NotSpecified, this);
        }
    }
}
