namespace AzureDevOpsMgmt.Resources
{
    using System;

    using AzureDevOpsMgmt.Authenticators;
    using AzureDevOpsMgmt.Models;
    using AzureDevOpsMgmt.Serialization;

    using RestSharp;

    using UTMO.PowerShell5.DI.Unity;

    public class DependencyContainer : PsUnityContainer
    {
        public DependencyContainer()
        {
            this.RegisterFactory<IRestClient, RestClient>(i => this.RestClientFactory());
        }

        private RestClient RestClientFactory()
        {
            var currentAccount = AzureDevOpsConfiguration.Config.CurrentConnection;
            var escapedProjectString = Uri.EscapeUriString(currentAccount.ProjectName);
            var client = new RestClient($"{currentAccount.Account.BaseUrl}/{escapedProjectString}/_apis");
            client.Authenticator = new BarerTokenAuthenticator();
            client.DefaultParameters.Add(new Parameter("api-version", "5.0", ParameterType.QueryString));
            client.DefaultParameters.Add(new Parameter("Accepts", "application/json", ParameterType.HttpHeader));
            client.DefaultParameters.Add(new Parameter("ContentType", "application/json", ParameterType.HttpHeader));
            client.UseSerializer(() => new JsonNetSerializer());
            return client;
        }
    }
}