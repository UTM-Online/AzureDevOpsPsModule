namespace AzureDevOpsMgmt.CoreTests.Models
{
    using AzureDevOpsMgmt.Cmdlets;

    public class TestCmdlet : ApiCmdlet
    {
        protected override void ProcessCmdletRecord()
        {
            this.WriteObject(this.client);
        }
    }
}