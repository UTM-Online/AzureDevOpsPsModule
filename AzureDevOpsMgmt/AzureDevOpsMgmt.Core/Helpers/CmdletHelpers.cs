// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="CmdletHelpers.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************

namespace AzureDevOpsMgmt.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Authenticators;
    using AzureDevOpsMgmt.Models;
    using AzureDevOpsMgmt.Serialization;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    using RestSharp;

    using UTMO.Common.Guards;

    /// <summary>
    /// Class CmdletHelpers.
    /// </summary>
    [SuppressMessage("ReSharper", "StyleCop.SA1650", Justification = "Suppression Approved")]
    public static class CmdletHelpers
    {
        /// <summary>
        /// Use this method to build a standardized ID for error records returned to the user.
        /// </summary>
        /// <param name="cmdlet">The cmdlet executing at the time the error was encountered.</param>
        /// <param name="resourceTarget">The type of resources being operated on at the time the error occured</param>
        /// <param name="errorReason">The Reason the error occured.  If no reason is provided then "UnknownError" will be used
        /// instead.</param>
        /// <returns>The standard error ID</returns>
        /// <remarks>The standard ID will be formatted as such: "AzureDevOpsMgmt.Cmdlet.{Resource Target Type}.{Cmdlet Class
        /// Name}.{Failure Reason}</remarks>
        public static string BuildStandardErrorId(
            this PSCmdlet cmdlet,
            DevOpsModelTarget resourceTarget,
            string errorReason = "UnknownError") =>
            CmdletHelpers.BuildStandardErrorIdInternal(
                                                       $"{resourceTarget.ToString()}.{cmdlet.GetType().Name}.{errorReason}");

        /// <summary>
        /// Gets the ps bound parameter.
        /// </summary>
        /// <typeparam name="T">The type of the value stored as a bound parameter</typeparam>
        /// <param name="cmdlet">The cmdlet.</param>
        /// <param name="name">The name.</param>
        /// <returns>The value of the PSBoundParameter</returns>
        public static T GetPsBoundParameter<T>(this PSCmdlet cmdlet, string name) =>
            (T)cmdlet.MyInvocation.BoundParameters[name];

        /// <summary>
        /// Gets the ps variable.
        /// </summary>
        /// <typeparam name="T">The type of the variable being retrieved</typeparam>
        /// <param name="cmdlet">The cmdlet.</param>
        /// <param name="name">The name.</param>
        /// <returns>The value of the variable</returns>
        /// <exception cref="T:System.Management.Automation.ProviderNotFoundException">If the <paramref name="name" /> refers to a
        /// provider that could not be found.</exception>
        /// <exception cref="T:System.Management.Automation.DriveNotFoundException">If the <paramref name="name" /> refers to a
        /// drive that could not be found.</exception>
        /// <exception cref="T:System.Management.Automation.ProviderInvocationException">If the provider threw an exception.</exception>
        public static T GetPsVariable<T>(this PSCmdlet cmdlet, string name) =>
            (T)cmdlet.SessionState.PSVariable.GetValue(name);

        /// <summary>
        /// Gets the rest client.
        /// </summary>
        /// <param name="_">The calling cmdlet</param>
        /// <returns>The Rest API Client</returns>
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

        /// <summary>
        /// Invokes the module cmdlet.
        /// </summary>
        /// <typeparam name="T">The type to be returned to the calling cmdlet</typeparam>
        /// <param name="cmdlet">The cmdlet.</param>
        /// <param name="commandName">Name of the command.</param>
        /// <param name="commandArgs">The command arguments.</param>
        /// <returns>The Collection of T.</returns>
        public static Collection<T> InvokeModuleCmdlet<T>(
            this PSCmdlet cmdlet,
            string commandName,
            IDictionary commandArgs = null) =>
            cmdlet.InvokePsCommand<T>($"AzureDevOpsMgmt\\{commandName}", commandArgs);

        /// <summary>
        /// Invokes the module cmdlet.
        /// </summary>
        /// <param name="cmdlet">The cmdlet.</param>
        /// <param name="commandName">Name of the command.</param>
        /// <param name="commandArgs">The command arguments.</param>
        public static void InvokeModuleCmdlet(this PSCmdlet cmdlet, string commandName, IDictionary commandArgs = null)
        {
            cmdlet.InvokePsCommand($"AzureDevOpsMgmt\\{commandName}", commandArgs);
        }

        /// <summary>
        /// Invokes the ps command.
        /// </summary>
        /// <typeparam name="T">The type of collection to be returned as a result of the cmdlet execution.</typeparam>
        /// <param name="_">The calling cmdlet</param>
        /// <param name="commandName">Name of the command.</param>
        /// <param name="commandArgs">The command arguments.</param>
        /// <returns>A Collection&lt;T&gt;.</returns>
        [SuppressMessage("ReSharper", "ExceptionNotDocumented", Justification = "Suppression Approved")]
        public static Collection<T> InvokePsCommand<T>(
            this PSCmdlet _,
            string commandName,
            IDictionary commandArgs = null)
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

        /// <summary>
        /// Invokes the ps command.
        /// </summary>
        /// <param name="_">The calling cmdlet</param>
        /// <param name="commandName">Name of the command.</param>
        /// <param name="commandArgs">The command arguments.</param>
        [SuppressMessage("ReSharper", "ExceptionNotDocumented", Justification = "Suppression Approved")]
        public static void InvokePsCommand(this object _, string commandName, IDictionary commandArgs = null)
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

        /// <summary>
        /// Sets the ps variable.
        /// </summary>
        /// <param name="cmdlet">The cmdlet.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="T:System.Management.Automation.SessionStateOverflowException">If the maximum number of variables has
        /// been reached for this scope.</exception>
        /// <exception cref="T:System.Management.Automation.ProviderNotFoundException">If the <paramref name="name" /> refers to a
        /// provider that could not be found.</exception>
        /// <exception cref="T:System.Management.Automation.DriveNotFoundException">If the <paramref name="name" /> refers to a
        /// drive that could not be found.</exception>
        /// <exception cref="T:System.Management.Automation.ProviderInvocationException">If the provider threw an exception.</exception>
        /// <exception cref="T:System.Management.Automation.SessionStateUnauthorizedAccessException">If the variable is read-only
        /// or constant.</exception>
        /// <exception cref="T:System.ArgumentNullException">If <paramref name="name" /> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">If the provider that the <paramref name="name" /> refers to does
        /// not support this operation.</exception>
        public static void SetPsVariable(this PSCmdlet cmdlet, string name, object value)
        {
            cmdlet.SessionState.PSVariable.Set(name, value);
        }

        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="cmdlet">The cmdlet.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="errorId">The error identifier.</param>
        /// <param name="category">The category.</param>
        /// <param name="target">The target.</param>
        [SuppressMessage("ReSharper", "ExceptionNotDocumented", Justification = "Suppression Approved")]
        public static void WriteError(
            this PSCmdlet cmdlet,
            Exception exception,
            string errorId,
            ErrorCategory category,
            object target)
        {
            var er = new ErrorRecord(exception, errorId, category, target);
            cmdlet.WriteError(er);
        }

        /// <summary>
        /// Updates the work item.
        /// </summary>
        /// <param name="cmdlet">The cmdlet.</param>
        /// <param name="original">The original work item.</param>
        /// <param name="updated">The updated updated work item.</param>
        /// <returns>A WorkItem.</returns>
        public static WorkItem UpdateWorkItem(this PSCmdlet cmdlet, WorkItem original, WorkItem updated)
        {
            Guard.Requires<InvalidOperationException>(
                                                      original.Id == updated.Id,
                                                      "Original Work Item Id and Updated Work Item Id do not match.");

            var cmdletParams = cmdlet.CreateParamDictionary().AddParam("Id", original.Id)
                                     .AddParam("UpdatedWorkItem", updated).AddParam("OriginalWorkItem", original);

            return cmdlet.InvokeModuleCmdlet<WorkItem>("Update-WorkItem", cmdletParams).First();
        }

        /// <summary>
        /// Gets the work item.
        /// </summary>
        /// <param name="cmdlet">The cmdlet.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>A WorkItem.</returns>
        public static WorkItem GetWorkItem(this PSCmdlet cmdlet, long id)
        {
            var cmdletParams = cmdlet.CreateParamDictionary().AddParam("Id", id);

            return cmdlet.InvokeModuleCmdlet<WorkItem>("Get-WorkItem", cmdletParams).First();
        }

        /// <summary>
        /// Creates the parameter dictionary.
        /// </summary>
        /// <param name="_">The Cmdlet.</param>
        /// <returns>A Dictionary of String Objects</returns>
        public static Dictionary<string, object> CreateParamDictionary(this PSCmdlet _) =>
            new Dictionary<string, object>();

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="cmdletParams">The cmdlet parameters.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns>Dictionary String Object</returns>
        public static Dictionary<string, object> AddParam(
            this Dictionary<string, object> cmdletParams,
            string name,
            object value)
        {
            cmdletParams.Add(name, value);
            return cmdletParams;
        }

        /// <summary>
        /// Builds the standard error identifier internal.
        /// </summary>
        /// <param name="resourceErrorString">The resource error string.</param>
        /// <returns>The standard base string plus the variable components.</returns>
        private static string BuildStandardErrorIdInternal(string resourceErrorString) =>
            $"AzureDevOpsMgmt.Cmdlet.{resourceErrorString}";
    }
}