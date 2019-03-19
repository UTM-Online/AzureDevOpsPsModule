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
            this.TokenId = tokenId;
            this.BaseUrl = baseUrl;
            this.AccountProjects = new List<string>();
        }

        /// <summary>Initializes a new instance of the <see cref="T:AzureDevOpsMgmt.Models.AzureDevOpsAccount"/> class.</summary>
        public AzureDevOpsAccount()
        {
        }

        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>The name of the friendly.</value>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        /// <value>The name of the account.</value>
        public string AccountName { get; set; }

        /// <summary>Gets or sets the account projects.</summary>
        /// <value>The account projects.</value>
        public List<string> AccountProjects { get; set; }

        /// <summary>
        /// Gets or sets the token identifier.
        /// </summary>
        /// <value>The token identifier.</value>
        public Guid? TokenId { get; set; }

        /// <summary>Gets or sets the base URL.</summary>
        /// <value>The base URL.</value>
        public string BaseUrl { get; set; }
    }
}
