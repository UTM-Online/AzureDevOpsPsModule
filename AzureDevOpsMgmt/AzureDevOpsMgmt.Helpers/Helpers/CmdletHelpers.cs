// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Helpers
// Author           : joirwi
// Created          : 03-18-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-18-2019
// ***********************************************************************
// <copyright file="CmdletHelpers.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace AzureDevOpsMgmt.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Authenticators;
    using AzureDevOpsMgmt.Models;

    using RestSharp;
    using RestSharp.Authenticators;

    /// <summary>
    /// Class CmdletHelpers.
    /// </summary>
    public static class CmdletHelpers
    {
        /// <summary>
        /// Gets the ps bound parameter.
        /// </summary>
        /// <typeparam name="T">The type of the value stored as a bound parameter</typeparam>
        /// <param name="cmdlet">The cmdlet.</param>
        /// <param name="name">The name.</param>
        /// <returns>The value of the PSBoundParameter</returns>
        public static T GetPsBoundParameter<T>(this PSCmdlet cmdlet, string name)
        {
            return (T)cmdlet.MyInvocation.BoundParameters[name];
        }

        /// <summary>Sets the ps variable.</summary>
        /// <param name="cmdlet">The cmdlet.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="T:System.Management.Automation.SessionStateOverflowException">If the maximum number of variables has been reached for this scope.</exception>
        /// <exception cref="T:System.Management.Automation.ProviderNotFoundException">If the <paramref name="name" /> refers to a provider that could not be found.</exception>
        /// <exception cref="T:System.Management.Automation.DriveNotFoundException">If the <paramref name="name" /> refers to a drive that could not be found.</exception>
        /// <exception cref="T:System.Management.Automation.ProviderInvocationException">If the provider threw an exception.</exception>
        /// <exception cref="T:System.Management.Automation.SessionStateUnauthorizedAccessException">If the variable is read-only or constant.</exception>
        /// <exception cref="T:System.ArgumentNullException">If <paramref name="name" /> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">
        /// If the provider that the <paramref name="name" /> refers to does
        /// not support this operation.
        /// </exception>
        public static void SetPsVariable(this PSCmdlet cmdlet, string name, object value)
        {
            cmdlet.SessionState.PSVariable.Set(name, value);
        }

        /// <summary>Gets the ps variable.</summary>
        /// <typeparam name="T">The type of the variable being retrieved</typeparam>
        /// <param name="cmdlet">The cmdlet.</param>
        /// <param name="name">The name.</param>
        /// <returns>The value of the variable</returns>
        /// <exception cref="T:System.Management.Automation.ProviderNotFoundException">If the <paramref name="name" /> refers to a provider that could not be found.</exception>
        /// <exception cref="T:System.Management.Automation.DriveNotFoundException">If the <paramref name="name" /> refers to a drive that could not be found.</exception>
        /// <exception cref="T:System.Management.Automation.ProviderInvocationException">If the provider threw an exception.</exception>
        public static T GetPsVariable<T>(this PSCmdlet cmdlet, string name)
        {
            return (T)cmdlet.SessionState.PSVariable.GetValue(name);
        }

        /// <summary>Gets the rest client.</summary>
        /// <param name="_">The calling cmdlet</param>
        /// <returns>  The Rest API Client</returns>
        public static RestClient GetRestClient(this PSCmdlet _)
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

        public static Collection<T> InvokePsCommand<T>(this PSCmdlet _, string commandName, IDictionary commandArgs = null)
        {
            using (var ps = PowerShell.Create(RunspaceMode.CurrentRunspace))
            {
                ps.AddCommand(commandName);

                if (commandArgs != null)
                {
                    ps.AddParameters(commandArgs);
                }

                return ps.Invoke<T>();
            }
        }

        public static void InvokePsCommand(this PSCmdlet _, string commandName, IDictionary commandArgs = null)
        {
            using (var ps = PowerShell.Create(RunspaceMode.CurrentRunspace))
            {
                ps.AddCommand(commandName);

                if (commandArgs != null)
                {
                    ps.AddParameters(commandArgs);
                }

                ps.Invoke();
            }
        }
    }
}
