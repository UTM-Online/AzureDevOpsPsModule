// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="RemoveAccountProject.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************

namespace AzureDevOpsMgmt.Cmdlets.Accounts
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    /// <summary>
    /// Class RemoveAccountProject.
    /// Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    [Cmdlet(VerbsCommon.Remove, "AccountProject")]
    public class RemoveAccountProject : PSCmdlet
    {
        /// <summary>
        /// Gets or sets the name of the account friendly.
        /// </summary>
        /// <value>The name of the account friendly.</value>
        [Parameter]
        public string AccountFriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        [Parameter]
        public string ProjectName { get; set; }

        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        protected override void ProcessRecord()
        {
            var account = AzureDevOpsConfiguration.Config.Accounts.Accounts.First(i => i.FriendlyName.Equals(this.AccountFriendlyName, StringComparison.OrdinalIgnoreCase));

            account.RemoveProject(this.ProjectName);

            AzureDevOpsConfiguration.Config.Accounts.PerformAccountUpdate(account);
        }
    }
}
