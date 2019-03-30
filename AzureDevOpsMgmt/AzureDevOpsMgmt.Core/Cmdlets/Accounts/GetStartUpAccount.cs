namespace AzureDevOpsMgmt.Cmdlets.Accounts
{
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    [Cmdlet(VerbsCommon.Get, "StartUpAccount")]
    public class GetStartUpAccount : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            this.WriteObject(AzureDevOpsConfiguration.Config.Configuration);
        }
    }
}