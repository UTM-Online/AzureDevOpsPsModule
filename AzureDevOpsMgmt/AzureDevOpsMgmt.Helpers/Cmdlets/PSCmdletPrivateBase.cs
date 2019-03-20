namespace AzureDevOpsMgmt.Cmdlets
{
    using System;
    using System.Management.Automation;
    using System.Reflection;

    using AzureDevOpsMgmt.Helpers;

    using RestSharp;

    public abstract class PSCmdletPrivateBase : PSCmdlet
    {
        /// <summary>
        /// The client
        /// </summary>
        protected RestClient client;

        protected override void BeginProcessing()
        {
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
    }
}
