// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="AddAccount.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************

namespace AzureDevOpsMgmt.Cmdlets.Accounts
{
    using System.Diagnostics.CodeAnalysis;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    /// <summary>
    /// Class AddAccount.
    /// Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    [Cmdlet(VerbsCommon.Add, "Account")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Required for PSCmdlets")]
    public class AddAccount : PSCmdlet
    {
        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        /// <value>The name of the account.</value>
        [Parameter]
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>The name of the friendly.</value>
        [Parameter]
        public string FriendlyName { get; set; }

        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        /// <exception cref="T:AzureDevOpsMgmt.Exceptions.ObjectExistsException">This exception is thrown if the user attempts to add an account and an existing account with that name or friendly name is found in the
        /// repository.</exception>
        protected override void ProcessRecord()
        {
            AzureDevOpsConfiguration.Config.Accounts.AddAccount(this.FriendlyName, this.AccountName);
        }
    }
}
