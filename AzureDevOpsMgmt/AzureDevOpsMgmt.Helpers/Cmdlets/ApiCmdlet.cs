namespace AzureDevOpsMgmt.Cmdlets
{
    using System;
    using System.Management.Automation;
    using System.Reflection;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;

    using Newtonsoft.Json;

    using RestSharp;

    public abstract class ApiCmdlet : PSCmdlet
    {
        /// <summary>
        /// The client
        /// </summary>
        protected RestClient client;

        protected sealed override void BeginProcessing()
        {
            if (!AzureDevOpsConfiguration.Config.ReadyForCommands)
            {
                this.ThrowTerminatingError(new ErrorRecord(new InvalidOperationException("Account context has not been set.  Please run \"Set-AzureDevOpsAccountContext\" before continuing."), "AzureDevOps.Cmdlet.Auth.AccountContextNotSetException", ErrorCategory.AuthenticationError, this));
            }

            this.client = this.GetRestClient();
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

        public void WriteObject<T>(IRestResponse<T> response, DevOpsModelTarget onErrorTarget, ErrorCategory onErrorCategory, object onErrorTargetObject, string onErrorReason = null)
        {
            if (response.IsSuccessful)
            {
                this.WriteObject(response.Data);
            }
            else
            {
                switch (response.ErrorException)
                {
                    case JsonSerializationException jse:
                        {
                            this.WriteError(response.ErrorException, this.BuildStandardErrorId(DevOpsModelTarget.Build, "ObjectDeserializationFailed"), ErrorCategory.ReadError, onErrorTargetObject);
                            break;
                        }
                    default:
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

                            break;
                        }
                }
            }
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
