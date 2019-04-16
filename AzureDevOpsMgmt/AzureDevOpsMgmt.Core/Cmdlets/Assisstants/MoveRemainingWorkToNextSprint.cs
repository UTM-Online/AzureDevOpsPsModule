namespace AzureDevOpsMgmt.Cmdlets.Assisstants
{
    using System;
    using System.Collections.Generic;
    using System.Management.Automation;
    using Helpers;
    using Microsoft.TeamFoundation.Work.WebApi;
    using Models;
    using Resources;
    using RestSharp;

    public class MoveRemainingWorkToNextSprint : ApiCmdlet
    {
        [Parameter(Mandatory = true)]
        public MoveOption MoveOption { get; set; }

        [Parameter]
        public string SourceSprint { get; set; }

        [Parameter]
        public string DestinationSprint { get; set; }

        protected override string OverrideApiPath => $"/{Uri.EscapeUriString(AzureDevOpsConfiguration.Config.CurrentConnection.ProjectName)}/_apis";

        protected override void BeginProcessingCmdlet()
        {
            var sourceSprintVerificationRequest = new RestRequest("/work/teamsettings/iterations");

            var sourceSprintVerificationResponse = this.client.Get<List<TeamSettingsIteration>>(sourceSprintVerificationRequest);

            if (sourceSprintVerificationResponse.IsSuccessful)
            {

            }
            else
            {
                this.WriteError(sourceSprintVerificationResponse.ErrorException, this.BuildStandardErrorId(DevOpsModelTarget.WorkItem), ErrorCategory.InvalidResult, this);
            }
        }
    }
}
