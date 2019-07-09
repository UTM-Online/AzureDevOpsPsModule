namespace AzureDevOpsMgmt.Cmdlets.WorkItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Authenticators;
    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;
    using AzureDevOpsMgmt.Serialization;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    using RestSharp;

    [Cmdlet(VerbsCommon.Search, "WorkItems")]
    public class SearchWorkItem : ApiCmdlet
    {
        [Parameter]
        public string TeamName { get; set; }

        [Parameter]
        public string[] Properties { get; set; }

        [Parameter]
        public string Query { get; set; }

        protected override IRestClient Client { get; set; }

        private List<WorkItem> workItems;

        private WorkItemQueryResult queryResults;

        protected override void BeginProcessingCmdlet()
        {
            var currentAccount = AzureDevOpsConfiguration.Config.CurrentConnection;
            var escapedProjectString = Uri.EscapeUriString(currentAccount.ProjectName);
            var escapedTeamString = Uri.EscapeUriString(this.TeamName);
            var client = new RestClient($"{currentAccount.Account.BaseUrl}/{escapedProjectString}/{escapedTeamString}/_apis");
            client.Authenticator = new BarerTokenAuthenticator();
            client.DefaultParameters.Add(new Parameter("api-version", "5.0", ParameterType.QueryString));
            client.DefaultParameters.Add(new Parameter("Accepts", "application/json", ParameterType.HttpHeader));
            client.DefaultParameters.Add(new Parameter("ContentType", "application/json", ParameterType.HttpHeader));
            client.UseSerializer(() => new JsonNetSerializer());

            this.Client = client;
        }

        protected override void ProcessCmdletRecord()
        {
            var request = new RestRequest("/wit/wiql");

            request.AddJsonBody(this.Query);

            var response = this.Client.Post<WorkItemQueryResult>(request);

            if (!response.IsSuccessful)
            {
                // TODO: Add error handling logic here
                return;
            }

            this.queryResults = response.Data;

            var queryFields = string.Join(",", this.queryResults.Columns.Select(i => i.Name));

            foreach (var item in this.queryResults.WorkItems)
            {
                var arguments = new Dictionary<string, object>() { { "Id", item.Id }, { "Fields", queryFields } };
                var workItem = this.InvokeModuleCmdlet<WorkItem>("Get-WorkItem", arguments).First();

                this.workItems.Add(workItem);
            }
        }

        protected override void EndCmdletProcessing()
        {
            if (this.workItems.Any())
            {
                this.WriteObject(this.workItems, true);
            }
            else
            {
                this.WriteWarning("Query returned no results!");
            }
        }
    }
}