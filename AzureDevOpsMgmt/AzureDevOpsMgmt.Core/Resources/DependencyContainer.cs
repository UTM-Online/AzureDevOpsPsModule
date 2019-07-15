// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 06-24-2019
// ***********************************************************************
// <copyright file="DependencyContainer.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Resources
{
    using System;

    using AzureDevOpsMgmt.Authenticators;
    using AzureDevOpsMgmt.Models;
    using AzureDevOpsMgmt.Serialization;

    using RestSharp;

    using UTMO.PowerShell5.DI.Unity;

    /// <summary>
    /// Class DependencyContainer.
    /// Implements the <see cref="UTMO.PowerShell5.DI.Unity.PsUnityContainer" />
    /// </summary>
    /// <seealso cref="UTMO.PowerShell5.DI.Unity.PsUnityContainer" />
    public class DependencyContainer : PsUnityContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyContainer"/> class.
        /// </summary>
        public DependencyContainer()
        {
            this.RegisterFactory<IRestClient, RestClient>(i => this.RestClientFactory());
            this.RegisterFactory<IRestClient, RestClient>(this.RestClientFactoryForTeams, "AdoTeamsApi");
        }

        /// <summary>
        /// Rests the client factory.
        /// </summary>
        /// <returns>A RestClient.</returns>
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

        /// <summary>
        /// Rests the client factory.
        /// </summary>
        /// <returns>A RestClient.</returns>
        private RestClient RestClientFactoryForTeams()
        {
            var currentAccount = AzureDevOpsConfiguration.Config.CurrentConnection;
            var escapedProjectString = Uri.EscapeUriString(currentAccount.ProjectName);
            var escapedTeamNameString = Uri.EscapeUriString(currentAccount.CurrentTeam);
            var client = new RestClient($"{currentAccount.Account.BaseUrl}/{escapedProjectString}/{escapedTeamNameString}/_apis");
            client.Authenticator = new BarerTokenAuthenticator();
            client.DefaultParameters.Add(new Parameter("api-version", "5.0", ParameterType.QueryString));
            client.DefaultParameters.Add(new Parameter("Accepts", "application/json", ParameterType.HttpHeader));
            client.DefaultParameters.Add(new Parameter("ContentType", "application/json", ParameterType.HttpHeader));
            client.UseSerializer(() => new JsonNetSerializer());
            return client;
        }
    }
}