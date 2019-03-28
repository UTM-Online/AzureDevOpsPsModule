namespace AzureDevOpsMgmt.Cmdlets.ReleaseManagement
{
    using System.Collections.Generic;
    using System.Management.Automation;
    using System.Management.Automation.Language;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;

    using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;

    using RestSharp;

    [Cmdlet(VerbsCommon.Get, "Release")]
    public class GetRelease : ApiCmdlet
    {
        [Parameter(ParameterSetName = "UserInput")]
        public int Id { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "UserInput")]
        public int DefinitionId { get; set; }

        [Parameter(ParameterSetName = "PipelineInput", DontShow = true, ValueFromPipeline = true)]
        public ReleaseDefinition PipelineInput { get; set; }

        protected override void BeginProcessing()
        {
            if (this.PipelineInput != null)
            {
                this.DefinitionId = this.PipelineInput.Id;
            }
        }

        protected override void ProcessRecord()
        {
            if (this.Id != default(int))
            {
                this.GetSingleRelease();
            }
            else
            {
                this.ListAllReleasesForDefinition();
            }
        }

        private void GetSingleRelease()
        {
            var request = new RestRequest($"release/releases/{this.Id}");

            var response = this.client.Get<Release>(request);

            if (response.IsSuccessful)
            {
                this.WriteObject(response.Data);
            }
            else
            {
                this.WriteError(response.ErrorException, this.BuildStandardErrorId(DevOpsModelTarget.Release), ErrorCategory.NotSpecified, this);
            }
        }

        private void ListAllReleasesForDefinition()
        {
            var request = new RestRequest("release/releases");
            request.AddQueryParameter("definitionId", this.DefinitionId.ToString());

            var response = this.client.Get<List<Release>>(request);

            if (response.IsSuccessful)
            {
                this.WriteObject(response.Data);
            }
            else
            {
                this.WriteError(response.ErrorException, this.BuildStandardErrorId(DevOpsModelTarget.Release), ErrorCategory.NotSpecified, this);
            }
        }
    }
}
