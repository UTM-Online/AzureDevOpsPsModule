// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Helpers
// Author           : joirwi
// Created          : 03-15-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="AzureDevOpsAccountCollection.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AzureDevOpsMgmt.Helpers.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;

    using AzureDevOpsMgmt.Helpers.Helpers;
    using AzureDevOpsMgmt.Helpers.Resources;

    /// <summary>
    /// Class AzureDevOpsAccountCollection.
    /// </summary>
    public class AzureDevOpsAccountCollection
    {
        /// <summary>Initializes a new instance of the <see cref="T:AzureDevOpsMgmt.Helpers.Models.AzureDevOpsAccountCollection"/> class.</summary>
        public AzureDevOpsAccountCollection()
        {
        }

        /// <summary>Initializes this instance.</summary>
        public void Init()
        {
            this.Accounts.CollectionChanged += this.OnCollectionChanged;
            this.PatTokens.CollectionChanged += this.OnCollectionChanged;
        }

        /// <summary>
        /// Gets or sets the accounts.
        /// </summary>
        /// <value>The accounts.</value>
        public ObservableCollection<AzureDevOpsAccount> Accounts { get; set; }

        /// <summary>
        /// Gets or sets the pat tokens.
        /// </summary>
        /// <value>The pat tokens.</value>
        public ObservableCollection<AzureDevOpsPatToken> PatTokens { get; set; }

        /// <summary>Adds the account.</summary>
        /// <param name="friendlyName">Name of the friendly.</param>
        /// <param name="accountName">Name of the account.</param>
        public void AddAccount(string friendlyName, string accountName)
        {
            var url = $"https://dev.azure.com/{accountName}/";
            var account = new AzureDevOpsAccount(friendlyName, accountName, url);

            this.Accounts.Add(account);
        }

        /// <summary>Adds the pat token.</summary>
        /// <param name="friendlyName">Name of the friendly.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="patToken">The pat token.</param>
        public void AddPatToken(string friendlyName, string userName, string patToken)
        {
            var protoToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{userName}:{patToken}"));
            var item = new AzureDevOpsPatToken(friendlyName, protoToken);
            this.PatTokens.Add(item);
        }

        /// <summary>Gets the account names.</summary>
        /// <returns>IEnumerable of string.</returns>
        public string[] GetAccountNames()
        {
            return this.Accounts.Select(i => i.FriendlyName).ToArray();
        }

        /// <summary>Removes the account.</summary>
        /// <param name="friendlyName">Name of the friendly.</param>
        public void RemoveAccount(string friendlyName)
        {
            this.Accounts.Remove(this.Accounts.First(i => i.FriendlyName == friendlyName));
        }

        /// <summary>Removes the pat token.</summary>
        /// <param name="friendlyName">Name of the friendly.</param>
        public void RemovePatToken(string friendlyName)
        {
            this.PatTokens.Remove(this.PatTokens.First(i => i.FriendlyName == friendlyName));
        }

        /// <summary>Links the pat token to account.</summary>
        /// <param name="accountFriendlyName">Name of the account friendly.</param>
        /// <param name="patTokenFriendlyName">Name of the pat token friendly.</param>
        public void LinkPatTokenToAccount(string accountFriendlyName, string patTokenFriendlyName)
        {
            Guid patTokenId = this.PatTokens.First(i => i.FriendlyName == patTokenFriendlyName).Id;

            var account = this.Accounts.First(i => i.FriendlyName == accountFriendlyName);

            // TODO: Make this an atomic transaction
            this.Accounts.Remove(account);
            account.TokenId = patTokenId;
            this.Accounts.Add(account);
        }

        /// <summary>Handles the <see cref="E:CollectionChanged"/> event.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            FileHelpers.WriteFileJson(FileNames.AccountData, this);
        }
    }
}
