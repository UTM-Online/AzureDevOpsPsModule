// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="AzureDevOpsConfiguration.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************

namespace AzureDevOpsMgmt.Models
{
    /// <summary>
    /// Class AzureDevOpsConfiguration.
    /// </summary>
    public class AzureDevOpsConfiguration
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public static AzureDevOpsConfiguration Config
        {
            get
            {
                if (AzureDevOpsConfiguration.PrivateConfig == null)
                {
                    AzureDevOpsConfiguration.PrivateConfig = new AzureDevOpsConfiguration();
                }

                return AzureDevOpsConfiguration.PrivateConfig;
            }
        }

        /// <summary>
        /// Gets or sets the accounts.
        /// </summary>
        /// <value>The accounts.</value>
        public AzureDevOpsAccountCollection Accounts { get; set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public UserConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the current connection.
        /// </summary>
        /// <value>The current connection.</value>
        public CurrentConnection CurrentConnection { get; set; }

        /// <summary>
        /// Gets a value indicating whether [ready for commands].
        /// </summary>
        /// <value><c>true</c> if [ready for commands]; otherwise, <c>false</c>.</value>
        public bool ReadyForCommands => this.CurrentConnection != null;

        /// <summary>
        /// Gets or sets the private configuration.
        /// </summary>
        /// <value>The private configuration.</value>
        private static AzureDevOpsConfiguration PrivateConfig { get; set; }
    }
}
