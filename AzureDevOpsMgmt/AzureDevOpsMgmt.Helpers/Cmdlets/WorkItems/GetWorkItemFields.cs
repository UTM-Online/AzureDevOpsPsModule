namespace AzureDevOpsMgmt.Cmdlets.WorkItems
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.WebApi.Jwt;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using RestSharp;

    [Cmdlet(VerbsCommon.Get, "WorkItemFields")]
    public class GetWorkItemFields : PSCmdletPrivateBase
    {
        [Parameter]
        public string FieldName { get; set; }

        protected override void ProcessRecord()
        {
            var request = new RestRequest($"wit/fields/{this.FieldName ?? string.Empty}", Method.GET);
            var response = this.client.Execute(request);

            if (response.IsSuccessful)
            {
                var jtoken = JToken.Parse(response.Content);

                var workItemFields = JsonConvert.DeserializeObject<List<WorkItemField>>(jtoken["value"].ToString());

                this.WriteObject(workItemFields);
            }
            else
            {
                this.WriteError(new ErrorRecord(response.ErrorException, "AzureDevOps.Cmdlets.GetAllWorkItemFields.UnknownError", ErrorCategory.NotSpecified, request));
            }
        }
    }
}
