// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Helpers
// Author           : joirwi
// Created          : 03-13-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="AzureDevOpsPatToken.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace AzureDevOpsMgmt.Models
{
    using System;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Resources;

    using Meziantou.Framework.Win32;

    using Newtonsoft.Json;

    /// <summary>
    /// Class AzureDevOpsPatToken.
    /// </summary>
    public class AzureDevOpsPatToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureDevOpsPatToken"/> class.
        /// </summary>
        public AzureDevOpsPatToken()
        {
            this.TokenValue = new Lazy<string>(() => CredentialManager.ReadCredential(this.CredentialManagerId).Password);
        }

        /// <summary>Initializes a new instance of the <see cref="T:AzureDevOpsMgmt.Models.AzureDevOpsPatToken"/> class.</summary>
        /// <param name="friendlyName">Name of the friendly.</param>
        /// <param name="tokenValue">The token value.</param>
        public AzureDevOpsPatToken(string friendlyName, string tokenValue)
        {
            this.FriendlyName = friendlyName;
            this.Id = Guid.NewGuid();
            this.TokenValue = new Lazy<string>(() => CredentialManager.ReadCredential(this.CredentialManagerId).Password);
            this.IsInScope = new Lazy<bool>(this.CheckIfMachineIsInScope);
            this.UpdateToken(tokenValue);
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the friendly.
        /// </summary>
        /// <value>The name of the friendly.</value>
        public string FriendlyName { get; set; }

        /// <summary>Gets or sets the created on machine.</summary>
        /// <value>The created on machine.</value>
        public Guid CreatedOnMachine { get; set; }

        /// <summary>Gets or sets the is in scope.</summary>
        /// <value>The is in scope.</value>
        [JsonIgnore]
        public Lazy<bool> IsInScope { get; set; }

        /// <summary>
        /// Gets or sets the token value.
        /// </summary>
        /// <value>The token value.</value>
        [JsonIgnore]
        public Lazy<string> TokenValue { get; set; }

        /// <summary>Gets the credential manager identifier.</summary>
        /// <value>The credential manager identifier.</value>
        private string CredentialManagerId => $"{StaticStrings.ApplicationName}_{this.Id}";

        /// <summary>Updates the token.</summary>
        /// <param name="newValue">The new value.</param>
        public void UpdateToken(string newValue)
        {
            CredentialManager.WriteCredential(this.CredentialManagerId, Environment.UserName, newValue, CredentialPersistence.LocalMachine);
        }

        /// <summary>Deletes the token.</summary>
        public void DeleteToken()
        {
            CredentialManager.DeleteCredential(this.CredentialManagerId);
        }
    }
}
