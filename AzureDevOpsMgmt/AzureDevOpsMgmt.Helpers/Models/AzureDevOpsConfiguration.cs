// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Helpers
// Author           : joirwi
// Created          : 03-15-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="AzureDevOpsConfiguration.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AzureDevOpsMgmt.Helpers.Models
{
    using System;
    using System.Linq;

    /// <summary>
    /// Class AzureDevOpsConfiguration.
    /// </summary>
    public class AzureDevOpsConfiguration
    {
        /// <summary>Initializes a new instance of the <see cref="T:AzureDevOpsMgmt.Helpers.Models.AzureDevOpsConfiguration"/> class.</summary>
        public AzureDevOpsConfiguration()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:AzureDevOpsMgmt.Helpers.Models.AzureDevOpsConfiguration"/> class.</summary>
        /// <param name="currentAccountId">The current account identifier.</param>
        /// <param name="collection">The collection.</param>
        public AzureDevOpsConfiguration(string currentAccountId, AzureDevOpsAccountCollection collection)
        {
            this.Accounts = collection;

            var currentAccount = this.Accounts.Accounts.First(a => a.FriendlyName == currentAccountId);
            var currentPat = this.Accounts.PatTokens.First(a => a.Id == currentAccount.TokenId);

            this.CurrentConnection = new Tuple<AzureDevOpsAccount, AzureDevOpsPatToken>(currentAccount, currentPat);
        }

        /// <summary>
        /// Gets or sets the accounts.
        /// </summary>
        /// <value>The accounts.</value>
        public AzureDevOpsAccountCollection Accounts { get; set; }

        /// <summary>
        /// Gets or sets the current connection.
        /// </summary>
        /// <value>The current connection.</value>
        public Tuple<AzureDevOpsAccount, AzureDevOpsPatToken> CurrentConnection { get; set; }

        /// <summary>Gets a value indicating whether [ready for commands].</summary>
        /// <value>
        /// <c>true</c> if [ready for commands]; otherwise, <c>false</c>.</value>
        public bool ReadyForCommands => this.CurrentConnection != null;

        /// <summary>Sets the current connection.</summary>
        /// <param name="accountName">Name of the account.</param>
        public void SetCurrentConnection(string accountName)
        {
            var newAccount = this.Accounts.Accounts.First(i => i.FriendlyName == accountName);
            var newPatToken = this.Accounts.PatTokens.First(i => i.Id == newAccount.TokenId);

            this.CurrentConnection = new Tuple<AzureDevOpsAccount, AzureDevOpsPatToken>(newAccount, newPatToken);
        }
    }
}
