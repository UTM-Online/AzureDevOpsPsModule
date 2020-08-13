// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 06-26-2019
// ***********************************************************************
// <copyright file="ApiCmdlet.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************

namespace AzureDevOpsMgmt.Cmdlets
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Management.Automation;
    using System.Reflection;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;

    using Newtonsoft.Json;

    using RestSharp;

    using UTMO.Powershell5.DI.CmdletBase;

    /// <summary>
    /// Class ApiCmdlet.
    /// Implements the <see cref="UTMO.Powershell5.DI.CmdletBase.DiBasePsCmdlet" />
    /// </summary>
    /// <seealso cref="UTMO.Powershell5.DI.CmdletBase.DiBasePsCmdlet" />
    public abstract class ApiCmdlet : DiBasePsCmdlet
    {
        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        [ShouldInject]
        protected virtual IRestClient Client { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is debug.
        /// </summary>
        /// <value><c>true</c> if this instance is debug; otherwise, <c>false</c>.</value>
        protected bool IsDebug
        {
            get
            {
                return this.MyInvocation.BoundParameters.ContainsKey("Debug");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is verbose.
        /// </summary>
        /// <value><c>true</c> if this instance is verbose; otherwise, <c>false</c>.</value>
        protected bool IsVerbose
        {
            get
            {
                return this.MyInvocation.BoundParameters.ContainsKey("Verbose");
            }
        }

        /// <summary>
        /// Gets or sets the override API path.
        /// </summary>
        /// <value>The override API path.</value>
        protected virtual string OverrideApiPath { get; set; }

        /// <summary>
        /// Writes the object.
        /// </summary>
        /// <typeparam name="T">The type of object to be written.</typeparam>
        /// <param name="response">The response.</param>
        /// <param name="onErrorTarget">The on error target.</param>
        /// <param name="onErrorCategory">The on error category.</param>
        /// <param name="onErrorTargetObject">The on error target object.</param>
        /// <param name="onErrorReason">The on error reason.</param>
        /// <param name="useExceptionTypeChecking">if set to <c>true</c> [use exception type checking].</param>
        [SuppressMessage("ReSharper", "StyleCop.SA1126", Justification = "Exceptions Documentation Not Needed.")]
        public void WriteObject<T>(
            IRestResponse<T> response,
            DevOpsModelTarget onErrorTarget,
            ErrorCategory onErrorCategory,
            object onErrorTargetObject,
            string onErrorReason = null,
            bool useExceptionTypeChecking = true)
        {
            if (this.IsDebug)
            {
                // ReSharper disable once ExceptionNotDocumented
                this.SetPsVariable("ResponseBody", response);
            }

            if (response.IsSuccessful)
            {
                var responseType = response.Data.GetType();
                if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(ResponseModel<>))
                {
                    // ReSharper disable once ExceptionNotDocumented
                    var responseObject = responseType.GetProperty("Value")?.GetValue(response.Data);

                    // ReSharper disable once ExceptionNotDocumented
                    this.WriteObject(responseObject, true);
                }
                else
                {
                    // ReSharper disable once ExceptionNotDocumented
                    this.WriteObject(response.Data);
                }
            }
            else
            {
                if (useExceptionTypeChecking)
                {
                    this.ProcessErrorResponse(response, onErrorTarget, onErrorCategory, onErrorTargetObject, onErrorReason);
                }
                else
                {
                    this.WriteErrorInternal(
                        response,
                        onErrorTarget,
                        onErrorCategory,
                        onErrorTargetObject,
                        onErrorReason);
                }
            }
        }

        /// <summary>
        /// Processes the error response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="onErrorTarget">The on error target.</param>
        /// <param name="onErrorCategory">The on error category.</param>
        /// <param name="onErrorTargetObject">The on error target object.</param>
        /// <param name="onErrorReason">The on error reason.</param>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1126:PrefixCallsCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        protected void ProcessErrorResponse(
            IRestResponse response,
            DevOpsModelTarget onErrorTarget,
            ErrorCategory onErrorCategory,
            object onErrorTargetObject,
            string onErrorReason = null)
        {
            switch (response.ErrorException)
            {
                case JsonSerializationException jse:
                    {
                        if (jse.Message.StartsWith("Cannot deserialize the current JSON object"))
                        {
                            this.WriteError(
                                response.ErrorException,
                                this.BuildStandardErrorId(onErrorTarget, "ObjectDeserializationFailed"),
                                ErrorCategory.ReadError,
                                onErrorTargetObject);
                            break;
                        }

                        goto default;
                    }

                case JsonReaderException jre:
                    {
                        if (jre.Message.StartsWith("Unexpected character encountered while parsing value:")
                            && response.Content.Contains("Access Denied: The Personal Access Token used has expired."))
                        {
                            this.WriteError(
                                response.ErrorException,
                                this.BuildStandardErrorId(onErrorTarget, "Your PAT Token has expired!!"),
                                ErrorCategory.AuthenticationError,
                                onErrorTargetObject);
                            break;
                        }

                        goto default;
                    }

                default:
                    {
                        this.WriteErrorInternal(response, onErrorTarget, onErrorCategory, onErrorTargetObject, onErrorReason);

                        break;
                    }
            }
        }

        /// <summary>
        /// Begins the cmdlet processing.
        /// </summary>
        protected override sealed void BeginCmdletProcessing()
        {
            if (!AzureDevOpsConfiguration.Config.ReadyForCommands)
            {
                // ReSharper disable once ExceptionNotDocumented
                this.ThrowTerminatingError(
                    new ErrorRecord(
                        new InvalidOperationException(
                            "Account context has not been set.  Please run \"Set-AzureDevOpsAccountContext\" before continuing."),
                        "AzureDevOps.Cmdlet.Auth.AccountContextNotSetException",
                        ErrorCategory.AuthenticationError,
                        this));
            }

            if (this.OverrideApiPath != null)
            {
                var currentAccount = AzureDevOpsConfiguration.Config.CurrentConnection;
                var escapedProjectString = Uri.EscapeUriString(currentAccount.ProjectName);

                this.Client.BaseUrl = new Uri(
                    $"{currentAccount.Account.BaseUrl}/{escapedProjectString}{this.OverrideApiPath}");
            }

            AppDomain.CurrentDomain.AssemblyResolve += this.CurrentDomain_BindingRedirect;

            this.BeginProcessingCmdlet();
        }

        /// <summary>
        /// Begins the processing cmdlet.
        /// </summary>
        protected virtual void BeginProcessingCmdlet()
        {
        }

        /// <summary>
        /// Handles the BindingRedirect event of the CurrentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="ResolveEventArgs" /> instance containing the event data.</param>
        /// <returns>An Assembly.</returns>
        private Assembly CurrentDomain_BindingRedirect(object sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name);

            switch (name.Name)
            {
                case "Newtonsoft.Json":
                    return typeof(JsonSerializer).Assembly;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Writes the error internal.
        /// </summary>
        /// <typeparam name="T">The type of response</typeparam>
        /// <param name="response">The response.</param>
        /// <param name="onErrorTarget">The on error target.</param>
        /// <param name="onErrorCategory">The on error category.</param>
        /// <param name="onErrorTargetObject">The on error target object.</param>
        /// <param name="onErrorReason">The on error reason.</param>
        private void WriteErrorInternal<T>(
            IRestResponse<T> response,
            DevOpsModelTarget onErrorTarget,
            ErrorCategory onErrorCategory,
            object onErrorTargetObject,
            string onErrorReason)
        {
            string errorId;

            if (string.IsNullOrWhiteSpace(onErrorReason))
            {
                errorId = this.BuildStandardErrorId(onErrorTarget);
            }
            else
            {
                errorId = this.BuildStandardErrorId(onErrorTarget, onErrorReason);
            }

            this.WriteError(response.ErrorException ?? new Exception("Unknown Error"), errorId, onErrorCategory, onErrorTargetObject);
        }

        /// <summary>
        /// Writes the error internal.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="onErrorTarget">The on error target.</param>
        /// <param name="onErrorCategory">The on error category.</param>
        /// <param name="onErrorTargetObject">The on error target object.</param>
        /// <param name="onErrorReason">The on error reason.</param>
        private void WriteErrorInternal(
            IRestResponse response,
            DevOpsModelTarget onErrorTarget,
            ErrorCategory onErrorCategory,
            object onErrorTargetObject,
            string onErrorReason)
        {
            string errorId;

            if (string.IsNullOrWhiteSpace(onErrorReason))
            {
                errorId = this.BuildStandardErrorId(onErrorTarget);
            }
            else
            {
                errorId = this.BuildStandardErrorId(onErrorTarget, onErrorReason);
            }

            this.WriteError(response.ErrorException ?? new Exception("Unknown Error"), errorId, onErrorCategory, onErrorTargetObject);
        }
    }
}