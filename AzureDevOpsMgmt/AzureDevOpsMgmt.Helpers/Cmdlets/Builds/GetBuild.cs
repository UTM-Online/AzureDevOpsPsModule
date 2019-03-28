namespace AzureDevOpsMgmt.Cmdlets.Builds
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using Microsoft.TeamFoundation.Build.WebApi;

    using RestSharp;

    // [Cmdlet(VerbsCommon.Get, "Build")]
    public class GetBuild : ApiCmdlet
    {
        [Parameter]
        public int BuildDefinitionId { get; set; }

        protected override void ProcessRecord()
        {
            var request = new RestRequest("build/builds", Method.GET);

            if (this.BuildDefinitionId != null)
            {
                request.AddParameter("definitions", this.BuildDefinitionId, ParameterType.QueryString);
            }

            var result = this.client.Execute<List<BuildDefinition>>(request);
        }
    }
}
