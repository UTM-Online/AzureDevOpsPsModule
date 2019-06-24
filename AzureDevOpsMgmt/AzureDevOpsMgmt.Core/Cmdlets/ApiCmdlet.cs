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

    using UTMO.Powershell5.DI.CmdletBase;

    public abstract class ApiCmdlet : DiBasePsCmdlet
    {
        /// <summary>
        /// The client
        /// </summary>
        [ShouldInject]
        protected RestClient client { get; set; }

        protected virtual string OverrideApiPath { get; set; }

        protected sealed override void BeginCmdletProcessing()
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
                    switch (response.ErrorException)
                    {
                        case JsonSerializationException jse:
                            {
                                if (jse.Message.StartsWith("Cannot deserialize the current JSON object"))
                                {
                                    this.WriteError(response.ErrorException, this.BuildStandardErrorId(DevOpsModelTarget.Build, "ObjectDeserializationFailed"), ErrorCategory.ReadError, onErrorTargetObject);
                                    break;
                                }
                                else
                                {
                                    goto default;
                                }
                            }
                        default:
                            {
                                this.WriteErrorInternal(response, onErrorTarget, onErrorCategory, onErrorTargetObject, onErrorReason);

                                break;
                            }
                    }
                }
                else
                {
                    this.WriteErrorInternal(response, onErrorTarget, onErrorCategory, onErrorTargetObject, onErrorReason);
                }
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
