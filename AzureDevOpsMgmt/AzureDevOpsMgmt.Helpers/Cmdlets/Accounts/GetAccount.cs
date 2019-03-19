// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Helpers
// Author           : joirwi
// Created          : 03-18-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-18-2019
// ***********************************************************************
// <copyright file="GetAccount.cs" company="Microsoft">
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
    /// Class GetAccount.
    /// Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    [Cmdlet(VerbsCommon.Get, "Account")]
    public class GetAccount : PSCmdlet
    {
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
        /// <exception cref="T:System.Management.Automation.PipelineStoppedException">
        ///             The pipeline has already been terminated, or was terminated
        ///             during the execution of this method.
        ///             The Cmdlet should generally just allow PipelineStoppedException
        ///             to percolate up to the caller of ProcessRecord etc.
        /// </exception>
        protected override void ProcessRecord()
        {
            if (string.IsNullOrWhiteSpace(this.FriendlyName))
            {
                this.WriteObject(AzureDevOpsConfiguration.Config.Accounts.Accounts, true);
            }

            this.WriteObject(AzureDevOpsConfiguration.Config.Accounts.Accounts.FirstOrDefault(i => i.FriendlyName.Equals(this.FriendlyName, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
