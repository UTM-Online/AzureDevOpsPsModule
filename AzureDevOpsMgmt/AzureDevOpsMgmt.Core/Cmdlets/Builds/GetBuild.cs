namespace AzureDevOpsMgmt.Cmdlets.Builds
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    using Microsoft.TeamFoundation.Build.WebApi;

    using RestSharp;

    [Cmdlet(VerbsCommon.Get, "Build", DefaultParameterSetName = "BuildDefinitionList")]
    public class GetBuild : ApiCmdlet
    {
        [Parameter(ParameterSetName = "SingleBuild")]
        public int BuildId { get; set; }

        [Parameter(ParameterSetName = "BuildDefinitionList")]
        public int[] BuildDefinitionId { get; set; }

        protected override void ProcessRecord()
        {
            var request = new RestRequest("build/builds", Method.GET);

            IRestResponse<ResponseModel<Build>> result;

            if (this.ParameterSetName == "BuildDefinitionList" && this.BuildDefinitionId == null)
            {
                result = this.client.Get<ResponseModel<Build>>(request);
            }
            else if (this.ParameterSetName == "BuildDefinitionList")
            {
                request.AddParameter("definitions", this.BuildDefinitionId, ParameterType.QueryString);
                result = this.client.Get<ResponseModel<Build>>(request);
            }
            else
            {
                request.Resource += $"/{this.BuildId}";
                result = this.client.Get<ResponseModel<Build>>(request);
            }

            this.WriteObject(result, DevOpsModelTarget.Build, ErrorCategory.NotSpecified, this);
        }
    }
}
