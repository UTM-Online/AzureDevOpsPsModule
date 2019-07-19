// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Helpers
// Author           : joirwi
// Created          : 03-18-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-18-2019
// ***********************************************************************
// <copyright file="CurrentConnection.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace AzureDevOpsMgmt.Models
{
    /// <summary>
    /// Class CurrentConnection.
    /// </summary>
    public class CurrentConnection
    {
        /// <summary>Initializes a new instance of the <see cref="T:AzureDevOpsMgmt.Models.CurrentConnection"/> class.</summary>
        /// <param name="account">The account.</param>
        /// <param name="token">The token.</param>
        /// <param name="projectName">Name of the project.</param>
        public CurrentConnection(AzureDevOpsAccount account, AzureDevOpsPatToken token, string projectName)
        {
            this.Account = account;
            this.Token = token;
            this.ProjectName = projectName;
        }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>The account.</value>
        public AzureDevOpsAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public AzureDevOpsPatToken Token { get; set; }

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        public string ProjectName { get; set; }

        /// <summary>Gets the current team.</summary>
        /// <value>The current team.</value>
        public string CurrentTeam { get; internal set; }
    }
}
