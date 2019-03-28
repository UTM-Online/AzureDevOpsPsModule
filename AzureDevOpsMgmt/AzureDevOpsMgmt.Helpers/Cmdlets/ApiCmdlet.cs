namespace AzureDevOpsMgmt.Cmdlets
{
    using System;
    using System.Management.Automation;
    using System.Reflection;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;

    using RestSharp;

    public abstract class ApiCmdlet : PSCmdlet
    {
        /// <summary>
        /// The client
        /// </summary>
        protected RestClient client;

        protected override void BeginProcessing()
        {
            if (!AzureDevOpsConfiguration.Config.ReadyForCommands)
            {
                this.ThrowTerminatingError(new ErrorRecord(new InvalidOperationException("Account context has not been set.  Please run \"Set-AzureDevOpsAccountContext\" before continuing."), "AzureDevOps.Cmdlet.Auth.AccountContextNotSetException", ErrorCategory.AuthenticationError, this));
            }

            this.client = this.GetRestClient();
            AppDomain.CurrentDomain.AssemblyResolve += this.CurrentDomain_BindingRedirect;
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
                this.WriteError(response.ErrorException, this.BuildStandardErrorId(onErrorTarget, onErrorReason), onErrorCategory, onErrorTargetObject);
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
