// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Helpers
// Author           : joirwi
// Created          : 03-18-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-18-2019
// ***********************************************************************
// <copyright file="AddPatToken.cs" company="Microsoft">
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
    /// Class AddPatToken.
    /// Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    [Cmdlet(VerbsCommon.Add, "PatToken")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Required for Cmdlets")]
    public class AddPatToken : PSCmdlet
    {
        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>The name of the friendly.</value>
        [Parameter]
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        [Parameter]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the pat token.
        /// </summary>
        /// <value>The pat token.</value>
        [Parameter]
        public string PatToken { get; set; }

        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        /// <exception cref="T:AzureDevOpsMgmt.Exceptions.ObjectExistsException">
        /// This exception is thrown if the user attempts to add a Pat Token and an existing Token with that friendly name is found in the
        ///     repository.
        /// </exception>
        protected override void ProcessRecord()
        {
            AzureDevOpsConfiguration.Config.Accounts.AddPatToken(this.FriendlyName, this.UserName, this.PatToken);
        }
    }
}
