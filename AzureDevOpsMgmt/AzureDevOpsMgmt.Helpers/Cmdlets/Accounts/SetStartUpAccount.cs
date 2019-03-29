namespace AzureDevOpsMgmt.Cmdlets.Accounts
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;
    using AzureDevOpsMgmt.Resources;

    [Cmdlet(VerbsCommon.Set, "StartUpAccount")]
    public class SetStartUpAccount : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string AccountFriendlyName { get; set; }

        [Parameter(Mandatory = true)]
        public string ProjectName { get; set; }

        protected override void EndProcessing()
        {
            FileHelpers.WriteFileJson(FileNames.UserData, AzureDevOpsConfiguration.Config.Configuration);
        }

        protected override void ProcessRecord()
        {
            var accountObject =
                AzureDevOpsConfiguration.Config.Accounts.Accounts.First(
                                                                        a => a.FriendlyName == this.AccountFriendlyName);

            var projectName =
                accountObject.AccountProjects.First(
                                                    a => a.Equals(this.ProjectName, StringComparison.OrdinalIgnoreCase));

            AzureDevOpsConfiguration.Config.Configuration.DefaultAccount = accountObject.FriendlyName;
            AzureDevOpsConfiguration.Config.Configuration.DefaultProject = projectName;
        }
    }
}
