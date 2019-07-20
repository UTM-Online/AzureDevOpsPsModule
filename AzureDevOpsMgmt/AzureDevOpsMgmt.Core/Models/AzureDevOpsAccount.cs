// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Helpers
// Author           : joirwi
// Created          : 03-15-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="AzureDevOpsAccount.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AzureDevOpsMgmt.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AzureDevOpsMgmt.Exceptions;

    using Newtonsoft.Json;

    using UTMO.Common.Guards;

    /// <summary>
    /// Class AzureDevOpsAccount.
    /// </summary>
    public class AzureDevOpsAccount
    {
        /// <summary>Initializes a new instance of the <see cref="T:AzureDevOpsMgmt.Models.AzureDevOpsAccount"/> class.</summary>
        /// <param name="friendlyName">Name of the friendly.</param>
        /// <param name="accountName">Name of the account.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="tokenId">The token identifier.</param>
        public AzureDevOpsAccount(string friendlyName, string accountName, string baseUrl, Guid? tokenId = null)
        {
            this.FriendlyName = friendlyName;
            this.AccountName = accountName;
            this.BaseUrl = baseUrl;
            this.InternalProjectsList = new List<string>();
            this.LinkedTokens = new List<Guid>();

            if (tokenId != null)
            {
                this.LinkedTokens.Add(tokenId.Value);
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:AzureDevOpsMgmt.Models.AzureDevOpsAccount"/> class.</summary>
        public AzureDevOpsAccount()
        {
        }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        /// <value>The name of the account.</value>
        public string AccountName { get; set; }

        /// <summary>Gets or sets the account projects.</summary>
        /// <value>The account projects.</value>
        [JsonIgnore]
        public IReadOnlyList<string> AccountProjects => this.InternalProjectsList;

        /// <summary>Gets the linked pat tokens.</summary>
        /// <value>The linked pat tokens.</value>
        [JsonIgnore]
        public IReadOnlyList<Guid> LinkedPatTokens => this.LinkedTokens;

        /// <summary>Gets or sets the base URL.</summary>
        /// <value>The base URL.</value>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>The name of the friendly.</value>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the token identifier.
        /// </summary>
        /// <value>The token identifier.</value>
        [Obsolete(null, false)]
        public Guid? TokenId { get; set; }

        /// <summary>Gets or sets the linked tokens.</summary>
        /// <value>The linked tokens.</value>
        [JsonProperty("LinkedTokens")]
        private List<Guid> LinkedTokens { get; set; }

        /// <summary>Gets or sets the internal projects list.</summary>
        /// <value>The internal projects list.</value>
        [JsonProperty(PropertyName = "AccountProjects")]
        private List<string> InternalProjectsList { get; set; }

        /// <summary>Adds the project to the account.</summary>
        /// <param name="name">The project name.</param>
        /// <exception cref="T:AzureDevOpsMgmt.Exceptions.ObjectExistsException">
        ///     This exception is thrown if the user tries to add a project that already exists in the account object.
        /// </exception>
        public void AddProject(string name)
        {
            if (this.InternalProjectsList.Any(p => p.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ObjectExistsException("Account Project");
            }

            this.InternalProjectsList.Add(name);
        }

        /// <summary>Removes the project from the account.</summary>
        /// <param name="name">The name.</param>
        public void RemoveProject(string name)
        {
            this.InternalProjectsList.Remove(name);
        }

        /// <summary>Adds the linked token.</summary>
        /// <param name="tokenId">The token identifier.</param>
        public void AddLinkedToken(Guid tokenId)
        {
            Guard.Requires<ArgumentNullException>(tokenId != default);
            Guard.Requires<InvalidOperationException>(!this.LinkedTokens.Contains(tokenId), "The Token specified is already linked to this account");

            this.LinkedTokens.Add(tokenId);
        }

        /// <summary>Removes the linked token.</summary>
        /// <param name="tokenId">The token identifier.</param>
        public void RemoveLinkedToken(Guid tokenId)
        {
            Guard.Requires<ArgumentNullException>(tokenId != default);
            Guard.Requires<InvalidOperationException>(this.LinkedTokens.Contains(tokenId), "The token specified is not linked to this account");

            this.LinkedTokens.Remove(tokenId);
        }

        /// <summary>Migrates the token to linked tokens.</summary>
        internal void MigrateTokenToLinkedTokens()
        {
            if (this.LinkedTokens == null)
            {
                this.LinkedTokens = new List<Guid>();
            }

            if (this.TokenId != null)
            {
                this.LinkedTokens.Add(this.TokenId.Value);
                this.TokenId = null;
            }
        }
    }
}
