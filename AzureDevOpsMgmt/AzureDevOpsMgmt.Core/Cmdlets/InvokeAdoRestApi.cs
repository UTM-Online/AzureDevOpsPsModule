namespace AzureDevOpsMgmt.Cmdlets
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using Helpers;
    using Models;
    using RestSharp;
    using UTMO.Common.Guards;

    [Cmdlet(VerbsLifecycle.Invoke, "AdoRestApi")]
    public class InvokeAdoRestApi : ApiCmdlet
    {
        [Parameter(Mandatory = true)]
        public string ResourcePath { get; set; }

        [Parameter(Mandatory = true)]
        public Method Action { get; set; }

        [Parameter(Mandatory = false)]
        public List<KeyValuePair<string,string>> QueryParameters { get; set; }

        [Parameter(Mandatory = false)]
        public object Body { get; set; }

        protected override void ProcessCmdletRecord()
        {
            var request = new RestRequest(this.ResourcePath, this.Action);

            if (this.QueryParameters != null && this.QueryParameters.Any())
            {
                foreach (var parameter in this.QueryParameters)
                {
                    request.AddQueryParameter(parameter.Key, parameter.Value);
                }
            }

            if (this.Body != null)
            {
                request.AddJsonBody(this.Body);
            }

            IRestResponse result = null;

            try
            {
                result = this.Client.ExecuteDynamic(request);
            }
            catch (Exception e)
            {
                this.WriteError(e, this.BuildStandardErrorId(DevOpsModelTarget.CustomTarget), ErrorCategory.NotSpecified, this.ResourcePath);
            }

            Guard.Requires<InvalidOperationException>(result != null, "Result was empty");

            this.WriteObject(result, true);
        }
    }
}