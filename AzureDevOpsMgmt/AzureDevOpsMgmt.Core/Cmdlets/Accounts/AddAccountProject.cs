// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Helpers
// Author           : joirwi
// Created          : 03-18-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-18-2019
// ***********************************************************************
// <copyright file="AddAccountProject.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace AzureDevOpsMgmt.Cmdlets.Accounts
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    /// <summary>
    /// Class AddAccountProject.
    /// Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    [Cmdlet(VerbsCommon.Add, "AccountProject")]
    public class AddAccountProject : PSCmdlet
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
        /// <exception cref="T:AzureDevOpsMgmt.Exceptions.ObjectExistsException">This exception is thrown if the user tries to add a project that already exists in the account object.</exception>
        protected override void ProcessRecord()
        {
            var account = AzureDevOpsConfiguration.Config.Accounts.Accounts.First(i => i.FriendlyName.Equals(this.AccountFriendlyName, StringComparison.OrdinalIgnoreCase));

            account.AddProject(this.ProjectName);

            AzureDevOpsConfiguration.Config.Accounts.PerformAccountUpdate(account);
        }
    }
}
