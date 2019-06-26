namespace AzureDevOpsMgmt.CoreTests.Models
{
    using System.Management.Automation;

    using AzureDevOpsMgmt.Cmdlets;

    [Cmdlet(VerbsDiagnostic.Test, "DiCmdlet")]
    public class TestCmdlet : ApiCmdlet
    {
        protected override void ProcessCmdletRecord()
        {
            this.WriteObject(this.client);
        }
    }
}