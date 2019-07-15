namespace AzureDevOpsMgmt.Cmdlets
{
    using System;
    using System.Management.Automation;
    using System.Reflection;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;

    using Newtonsoft.Json;

    using RestSharp;
    using RestSharp.Extensions;

    public abstract class ApiCmdlet : PSCmdlet
    {
        /// <summary>
        /// The client
        /// </summary>
        protected RestClient client;

        protected virtual string OverrideApiPath { get; set; }

        protected sealed override void BeginProcessing()
        {
            if (!AzureDevOpsConfiguration.Config.ReadyForCommands)
            {
                this.ThrowTerminatingError(new ErrorRecord(new InvalidOperationException("Account context has not been set.  Please run \"Set-AzureDevOpsAccountContext\" before continuing."), "AzureDevOps.Cmdlet.Auth.AccountContextNotSetException", ErrorCategory.AuthenticationError, this));
            }

            this.client = this.GetRestClient();

            if (this.OverrideApiPath != null)
            {
                var currentAccount = AzureDevOpsConfiguration.Config.CurrentConnection;
                var escapedProjectString = Uri.EscapeUriString(currentAccount.ProjectName);

                this.client.BaseUrl = new Uri($"{currentAccount.Account.BaseUrl}/{escapedProjectString}{this.OverrideApiPath}");
            }

            AppDomain.CurrentDomain.AssemblyResolve += this.CurrentDomain_BindingRedirect;

            this.BeginProcessingCmdlet();
        }

        protected virtual void BeginProcessingCmdlet()
        {
        }

        private Assembly CurrentDomain_BindingRedirect(object sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name);

            switch (name.Name)
            {
                case "Newtonsoft.Json":
                    return typeof(Newtonsoft.Json.JsonSerializer).Assembly;

                default:
                    return null;
            }
        }

        public void WriteObject<T>(IRestResponse<T> response, DevOpsModelTarget onErrorTarget, ErrorCategory onErrorCategory, object onErrorTargetObject, string onErrorReason = null, bool useExceptionTypeChecking = true)
        {
            if (this.IsDebug)
            {
                this.SetPsVariable("ResponseBody", response);
            }

            if (response.IsSuccessful)
            {
                var responseType = response.Data.GetType();
                if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(ResponseModel<>))
                {
                    var responseObject = responseType.GetProperty("Value").GetValue(response.Data);
                    this.WriteObject(responseObject, true);
                }
                else
                {
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
        ///     Begins the cmdlet processing.
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
        ///     Begins the processing cmdlet.
        /// </summary>
        protected virtual void BeginProcessingCmdlet()
        {
        }

        /// <summary>
        ///     Handles the BindingRedirect event of the CurrentDomain control.
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

            this.WriteError(response.ErrorException, errorId, onErrorCategory, onErrorTargetObject);
        }

        /// <summary>
        ///     Writes the error internal.
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

            this.WriteError(response.ErrorException, errorId, onErrorCategory, onErrorTargetObject);
        }

        protected bool IsVerbose
        {
            get { return this.MyInvocation.BoundParameters.ContainsKey("Verbose"); }
        }

        protected bool IsDebug
        {
            get { return this.MyInvocation.BoundParameters.ContainsKey("Debug"); }
        }
    }
}
