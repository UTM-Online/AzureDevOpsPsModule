﻿namespace AzureDevOpsMgmt.Cmdlets.Accounts
{
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    [Cmdlet(VerbsCommon.Get, "DefaultAccount")]
    public class GetDefaultAccount : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            this.WriteObject(AzureDevOpsConfiguration.Config.Configuration);
        }
    }
}