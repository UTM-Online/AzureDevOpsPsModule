namespace AzureDevOpsMgmt.Helpers.Cmdlets.Accounts
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Helpers.Models;

    using Microsoft.VisualStudio.Services.Organization.Client;

    [Cmdlet(VerbsCommon.Set, "AccountContext")]
    public class SetAccount : PSCmdlet, IDynamicParameters
    {
        public object GetDynamicParameters()
        {
            var parameterName = "AccountName";
            var runTimeParameterDictionary = new RuntimeDefinedParameterDictionary();
            var attributeCollection = new Collection<Attribute>();
            var parameterAttribute = new ParameterAttribute();
            parameterAttribute.Mandatory = true;
            parameterAttribute.Position = 1;
            attributeCollection.Add(parameterAttribute);
            var arraySet = AzureDevOpsConfiguration.Config.Accounts.GetAccountNames();
            var validateSetAttribute = new ValidateSetAttribute(arraySet);
            attributeCollection.Add(validateSetAttribute);
            var runtimeParameter = new RuntimeDefinedParameter(parameterName, typeof(string[]), attributeCollection);
            runTimeParameterDictionary.Add(parameterName, runtimeParameter);
            return runTimeParameterDictionary;
        }

        [Parameter]
        public string ProjectName { get; set; }

        protected override void BeginProcessing()
        {
            var foundAccounts =
                AzureDevOpsConfiguration.Config.Accounts.Accounts.FirstOrDefault(i => i.FriendlyName.Equals(this.MyInvocation.BoundParameters["AccountName"].ToString()));

            if (foundAccounts == null)
            {

            }
        }
    }
}
