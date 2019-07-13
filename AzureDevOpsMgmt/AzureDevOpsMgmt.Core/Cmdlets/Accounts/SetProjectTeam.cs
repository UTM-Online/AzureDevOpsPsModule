// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : josh
// Created          : 07-13-2019
//
// Last Modified By : josh
// Last Modified On : 07-13-2019
// ***********************************************************************
// <copyright file="SetProjectTeam.cs" company="UTM-Online">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace AzureDevOpsMgmt.Cmdlets.Accounts
{
    using System;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    using UTMO.Common.Guards;

    /// <summary>
    /// Class SetProjectTeam.
    /// Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    public class SetProjectTeam : PSCmdlet
    {
        /// <summary>
        /// Gets or sets the name of the team.
        /// </summary>
        /// <value>The name of the team.</value>
        [Parameter(Mandatory = true)]
        public string TeamName { get; set; }

        /// <summary>
        /// When overridden in the derived class, performs initialization
        /// of command execution.
        /// Default implementation in the base class just returns.
        /// </summary>
        protected override void BeginProcessing()
        {
            Guard.Requires<InvalidOperationException>(AzureDevOpsConfiguration.Config.ReadyForCommands, "You must select a project before you can select a team!");

            var currentConnection = AzureDevOpsConfiguration.Config.CurrentConnection;

            Guard.Requires<InvalidOperationException>(currentConnection.Account.AccountProjectsAndTeams[currentConnection.ProjectName].Contains(this.TeamName), "The requested team was not found to be a part of the current project");
        }

        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        protected override void ProcessRecord()
        {
            AzureDevOpsConfiguration.Config.CurrentConnection.CurrentTeam = this.TeamName;
        }

        /// <summary>
        /// When overridden in the derived class, performs clean-up
        /// after the command execution.
        /// Default implementation in the base class just returns.
        /// </summary>
        protected override void EndProcessing()
        {
            this.WriteObject($"Project Team Set To: {this.TeamName}");
        }
    }
}