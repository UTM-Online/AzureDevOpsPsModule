// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 06-07-2019
// ***********************************************************************
// <copyright file="SetStartUpAccount.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Cmdlets.Accounts
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;
    using AzureDevOpsMgmt.Resources;

    /// <summary>
    /// Class SetStartUpAccount.
    /// Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    [Cmdlet(VerbsCommon.Set, "StartUpAccount")]
    public class SetStartUpAccount : PSCmdlet
    {
        /// <summary>
        /// Gets or sets the name of the account friendly.
        /// </summary>
        /// <value>The name of the account friendly.</value>
        [Parameter(Mandatory = true)]
        public string AccountFriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        [Parameter(Mandatory = true)]
        public string ProjectName { get; set; }

        /// <summary>
        /// When overridden in the derived class, performs clean-up
        /// after the command execution.
        /// Default implementation in the base class just returns.
        /// </summary>
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        protected override void EndProcessing()
        {
            FileHelpers.WriteFileJson(FileNames.UserData, AzureDevOpsConfiguration.Config.Configuration);
        }

        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        protected override void ProcessRecord()
        {
            var accountObject =
                AzureDevOpsConfiguration.Config.Accounts.Accounts.First(
                                                                        a => a.FriendlyName == this.AccountFriendlyName);

            var projectName =
                accountObject.AccountProjects.First(
                                                    a => a.Equals(this.ProjectName, StringComparison.OrdinalIgnoreCase));

            AzureDevOpsConfiguration.Config.Configuration.DefaultAccount = accountObject.FriendlyName;
            AzureDevOpsConfiguration.Config.Configuration.DefaultProject = projectName;

            AzureDevOpsConfiguration.Config.Configuration.Save();
        }
    }
}
