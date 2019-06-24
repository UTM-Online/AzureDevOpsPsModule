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

        protected override void ProcessCmdletRecord()
        {
            if (this.FieldName != null)
            {
                var request = new RestRequest("wit/fields/{fieldName}");
                request.AddUrlSegment("fieldName", this.FieldName);
                var response = this.client.Get<WorkItemField>(request);
                this.WriteObject(response, DevOpsModelTarget.WorkItem, ErrorCategory.NotSpecified, this);
            }
            else
            {
                var request = new RestRequest("wit/fields", Method.GET);
                var response = this.client.Execute<ResponseModel<WorkItemField>>(request);
                this.WriteObject(response, DevOpsModelTarget.WorkItem, ErrorCategory.NotSpecified, this);
            }
        }
    }
}
