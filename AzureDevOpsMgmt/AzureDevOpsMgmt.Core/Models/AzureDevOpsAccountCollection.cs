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

namespace AzureDevOpsMgmt.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;

    using AzureDevOpsMgmt.Exceptions;
    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Resources;

    /// <summary>
    /// Class AzureDevOpsAccountCollection.
    /// </summary>
    public class AzureDevOpsAccountCollection
    {
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

        /// <summary>Gets or sets a value indicating whether this instance has completed 1905 upgrade.</summary>
        /// <value>
        ///   <c>true</c> if this instance has completed 1905 upgrade; otherwise, <c>false</c>.
        /// </value>
        public bool HasCompleted1905Upgrade { get; set; }

        /// <summary>Adds the account.</summary>
        /// <param name="friendlyName">Name of the friendly.</param>
        /// <param name="accountName">Name of the account.</param>
        /// <exception cref="T:AzureDevOpsMgmt.Exceptions.ObjectExistsException">
        ///     This exception is thrown if the user attempts to add an account and an existing account with that name or friendly name is found in the
        ///     repository.
        /// </exception>
        public void AddAccount(string friendlyName, string accountName)
        {
            if (this.Accounts.Any(
                                  a => a.AccountName.Equals(accountName, StringComparison.OrdinalIgnoreCase)
                                     | a.FriendlyName.Equals(friendlyName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ObjectExistsException("Account");
            }

            var url = $"https://dev.azure.com/{accountName}";
            var account = new AzureDevOpsAccount(friendlyName, accountName, url);

            this.Accounts.Add(account);
        }

        /// <summary>Adds the pat token.</summary>
        /// <param name="friendlyName">Name of the friendly.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="patToken">The pat token.</param>
        /// <exception cref="T:AzureDevOpsMgmt.Exceptions.ObjectExistsException">
        ///     This exception is thrown if the user attempts to add a Pat Token and an existing Token with that friendly name is found in the
        ///     repository.
        /// </exception>
        public void AddPatToken(string friendlyName, string userName, string patToken)
        {
            if (this.PatTokens.Any(p => p.FriendlyName.Equals(friendlyName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ObjectExistsException("Pat Token");
            }

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

        /// <summary>Initializes this instance.</summary>
        public void Init()
        {
            this.Accounts.CollectionChanged += this.OnCollectionChanged;
            this.PatTokens.CollectionChanged += this.OnCollectionChanged;
        }

        /// <summary>Links the pat token to account.</summary>
        /// <param name="accountFriendlyName">Name of the account friendly.</param>
        /// <param name="patTokenFriendlyName">Name of the pat token friendly.</param>
        internal void LinkPatTokenToAccount(string accountFriendlyName, string patTokenFriendlyName)
        {
            var patTokenId = this.PatTokens.First(i => i.FriendlyName.Equals(patTokenFriendlyName, StringComparison.OrdinalIgnoreCase)).Id;

            var account = this.Accounts.First(i => i.FriendlyName.Equals(accountFriendlyName, StringComparison.OrdinalIgnoreCase));
            account.TokenId = patTokenId;

            this.PerformAccountUpdate(i => i.FriendlyName == account.FriendlyName, account);
        }

        /// <summary>Performs the account transaction.</summary>
        /// <param name="selector">The selector.</param>
        /// <param name="updateData">The update data.</param>
        /// <returns>
        ///   <c>true</c> if operation succeed, <c>false</c> otherwise.
        /// </returns>
        internal bool PerformAccountUpdate(Func<AzureDevOpsAccount, bool> selector, AzureDevOpsAccount updateData)
        {
            var tempItem = this.Accounts.First(selector);

            try
            {
                this.Accounts.Remove(tempItem);
                this.Accounts.Add(updateData);
                return true;
            }
            catch (Exception)
            {
                // TODO: Add some logging here
                if (this.Accounts.Contains(tempItem))
                {
                    this.Accounts.Remove(tempItem);
                    this.Accounts.Add(tempItem);
                }
                else
                {
                    this.Accounts.Add(tempItem);
                }

                return false;
            }
        }

        /// <summary>Performs the pat token transaction.</summary>
        /// <param name="selector">The selector.</param>
        /// <param name="updateData">The update data.</param>
        /// <returns>
        /// <c>true</c> if operation was successful, <c>false</c> otherwise.
        /// </returns>
        internal bool PerformPatTokenUpdate(Func<AzureDevOpsPatToken, bool> selector, AzureDevOpsPatToken updateData)
        {
            var tempItem = this.PatTokens.First(selector);

            try
            {
                this.PatTokens.Remove(tempItem);
                this.PatTokens.Add(updateData);
                return true;
            }
            catch (Exception)
            {
                // TODO: Add some logging here
                if (this.PatTokens.Contains(tempItem))
                {
                    this.PatTokens.Remove(tempItem);
                    this.PatTokens.Add(tempItem);
                }
                else
                {
                    this.PatTokens.Add(tempItem);
                }

                return false;
            }
        }

        /// <summary>Removes the account.</summary>
        /// <param name="friendlyName">Name of the friendly.</param>
        internal void RemoveAccount(string friendlyName)
        {
            this.Accounts.Remove(this.Accounts.First(i => i.FriendlyName == friendlyName));
        }

        /// <summary>Removes the pat token.</summary>
        /// <param name="friendlyName">Name of the friendly.</param>
        internal void RemovePatToken(string friendlyName)
        {
            var token = this.PatTokens.First(i => i.FriendlyName == friendlyName);
            token.DeleteToken();
            this.PatTokens.Remove(token);
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
