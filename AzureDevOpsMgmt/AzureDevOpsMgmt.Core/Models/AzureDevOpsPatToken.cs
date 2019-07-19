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
    using System.ComponentModel;
    using System.Collections.Generic;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models.Contracts;
    using AzureDevOpsMgmt.Resources;

    using Meziantou.Framework.Win32;

    using Newtonsoft.Json;

    using UTMO.Common.Guards;

    /// <summary>
    ///     Class AzureDevOpsPatToken.
    /// </summary>
    public class AzureDevOpsPatToken : IPortable
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AzureDevOpsPatToken" /> class.
        /// </summary>
        public AzureDevOpsPatToken()
        {
            this.TokenValue =
                new Lazy<string>(() => CredentialManager.ReadCredential(this.CredentialManagerId).Password);

            this.NotOnMachines = new List<Guid>();
            this.MachineScopeId = Guid.Empty;
        }

        /// <summary>Initializes a new instance of the <see cref="T:AzureDevOpsMgmt.Models.AzureDevOpsPatToken" /> class.</summary>
        /// <param name="friendlyName">Name of the friendly.</param>
        /// <param name="tokenValue">The token value.</param>
        public AzureDevOpsPatToken(string friendlyName, string tokenValue) : this()
        {
            this.FriendlyName = friendlyName;
            this.Id = Guid.NewGuid();

            this.UpdateToken(tokenValue);
        }

        /// <summary>
        ///     Gets or sets the name of the friendly.
        /// </summary>
        /// <value>The name of the friendly.</value>
        public string FriendlyName { get; set; }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }

        /// <summary>Gets a value indicating whether this instance is in scope.</summary>
        /// <value>
        ///     <c>true</c> if this instance is in scope; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public bool IsInScope
        {
            get
            {
                return this.CheckIfMachineIsInScope();
            }
        }

        /// <summary>Gets or sets the machine scope identifier.</summary>
        /// <value>The machine scope identifier.</value>
        public Guid? MachineScopeId { get; set; }

        /// <summary>
        ///     Gets or sets the token value.
        /// </summary>
        /// <value>The token value.</value>
        [JsonIgnore]
        public Lazy<string> TokenValue { get; set; }

        /// <summary>Gets or sets the not on machines.</summary>
        /// <value>The not on machines.</value>
        [JsonProperty]
        internal List<Guid> NotOnMachines { get; set; }

        /// <summary>Gets the credential manager identifier.</summary>
        /// <value>The credential manager identifier.</value>
        internal string CredentialManagerId => $"{StaticStrings.ApplicationName}_{this.Id}";

        /// <summary>Updates the token.</summary>
        /// <param name="newValue">The new value.</param>
        public void UpdateToken(string newValue)
        {
            Guard.Requires<InvalidOperationException>(this.MachineScopeId == default || this.IsInScope, "You can not update a key created on another machine.");

            CredentialManager.WriteCredential(this.CredentialManagerId, Environment.UserName, newValue, CredentialPersistence.LocalMachine);
        }

        /// <summary>Deletes the token.</summary>
        public void DeleteToken()
        {
            Guard.Requires<InvalidOperationException>(this.MachineScopeId == default || this.IsInScope, "You can not delete a key created on another machine.");

            try
            {
                CredentialManager.DeleteCredential(this.CredentialManagerId);
            }
            catch (Win32Exception win32Exception) when(win32Exception.Message.Equals("Element not found", StringComparison.OrdinalIgnoreCase))
            {
                // Do nothing if we encounter an "Element not found" exception. This happens if the key was previously deleted and the operation did not
                // complete or if the credential entry was removed from the credential manager externally
            }
        }
    }
}