namespace AzureDevOpsMgmt.Cmdlets.WorkItems
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.WebApi.Jwt;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using RestSharp;

    [Cmdlet(VerbsCommon.Get, "WorkItemFields")]
    public class GetWorkItemFields : ApiCmdlet
    {
        [Parameter]
        public string FieldName { get; set; }

        protected override void ProcessRecord()
        {
            var request = new RestRequest($"wit/fields/{this.FieldName ?? string.Empty}", Method.GET);
            var response = this.client.Execute<ResponseModel<WorkItemField>>(request);

            this.WriteObject(response, DevOpsModelTarget.WorkItem, ErrorCategory.NotSpecified, this);
        }
    }
}
