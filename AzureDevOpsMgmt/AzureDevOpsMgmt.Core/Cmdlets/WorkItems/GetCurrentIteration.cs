namespace AzureDevOpsMgmt.Cmdlets.WorkItems
{
    using RestSharp;

    public class GetCurrentIteration : ApiCmdlet
    {
        [ShouldInject("AdoTeamsApi")]
        protected override IRestClient Client { get; set; }
    }
}