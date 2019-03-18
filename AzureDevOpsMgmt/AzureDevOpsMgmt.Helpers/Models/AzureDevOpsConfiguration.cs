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

    /// <summary>
    /// Class AzureDevOpsConfiguration.
    /// </summary>
    public class AzureDevOpsConfiguration
    {
        /// <summary>Gets the configuration.</summary>
        /// <value>The configuration.</value>
        public static AzureDevOpsConfiguration Config
        {
            get
            {
                if (PrivateConfig == null)
                {
                    PrivateConfig = new AzureDevOpsConfiguration();
                }

                return PrivateConfig;
            }
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
        public Tuple<AzureDevOpsAccount, AzureDevOpsPatToken, string> CurrentConnection { get; set; }

        /// <summary>Gets a value indicating whether [ready for commands].</summary>
        /// <value>
        /// <c>true</c> if [ready for commands]; otherwise, <c>false</c>.</value>
        public bool ReadyForCommands => this.CurrentConnection != null;

        /// <summary>Gets or sets the private configuration.</summary>
        /// <value>The private configuration.</value>
        private static AzureDevOpsConfiguration PrivateConfig { get; set; }
    }
}
