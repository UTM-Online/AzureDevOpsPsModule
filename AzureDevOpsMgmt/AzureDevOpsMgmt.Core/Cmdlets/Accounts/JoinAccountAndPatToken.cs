// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Helpers
// Author           : joirwi
// Created          : 03-18-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-18-2019
// ***********************************************************************
// <copyright file="JoinAccountAndPatToken.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AzureDevOpsMgmt.Cmdlets.Accounts
{
    using System.Diagnostics.CodeAnalysis;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    /// <summary>
    /// Class JoinAccountAndPatToken.
    /// Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    [Cmdlet(VerbsCommon.Join, "AccountAndPatToken")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Required for PSCmdlet")]
    public class JoinAccountAndPatToken : PSCmdlet
    {
        /// <summary>
        /// Gets or sets the name of the account friendly.
        /// </summary>
        /// <value>The name of the account friendly.</value>
        [Parameter]
        public string AccountFriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the pat token friendly.
        /// </summary>
        /// <value>The name of the pat token friendly.</value>
        [Parameter]
        public string PatTokenFriendlyName { get; set; }

        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        protected override void ProcessRecord()
        {
            AzureDevOpsConfiguration.Config.Accounts.LinkPatTokenToAccount(this.AccountFriendlyName, this.PatTokenFriendlyName);
        }
    }
}
