// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : josh
// Created          : 07-11-2019
//
// Last Modified By : josh
// Last Modified On : 07-11-2019
// ***********************************************************************
// <copyright file="AddProjectTeam.cs" company="UTM-Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Cmdlets.Accounts
{
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    /// <summary>
    /// Class AddProjectTeam.
    /// Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    public class AddProjectTeam : PSCmdlet
    {
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        [Parameter(Mandatory = true)]
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the name of the team.
        /// </summary>
        /// <value>The name of the team.</value>
        [Parameter(Mandatory = true)]
        public string TeamName { get; set; }

        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        protected override void ProcessRecord()
        {
            var currentSettings = AzureDevOpsConfiguration.Config.CurrentConnection;

            currentSettings.Account.AddProjectTeam(this.ProjectName, this.TeamName);
        }
    }
}