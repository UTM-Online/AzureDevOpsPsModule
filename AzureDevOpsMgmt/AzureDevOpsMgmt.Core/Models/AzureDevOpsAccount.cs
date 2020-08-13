// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Helpers
// Author           : joirwi
// Created          : 03-15-2019
// ***********************************************************************
// <copyright file="AzureDevOpsAccount.cs" company="UTM-Online">
//     Copyright ©  2019
// </copyright>
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
    ///     Class AzureDevOpsAccount.
    /// </summary>
    public class AzureDevOpsAccount
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AzureDevOpsAccount" /> class.
        /// </summary>
        /// <param name="friendlyName">Name of the friendly.</param>
        /// <param name="accountName">Name of the account.</param>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="tokenId">The token identifier.</param>
        public AzureDevOpsAccount(string friendlyName, string accountName, string baseUrl, Guid? tokenId = null)
        {
            this.FriendlyName = friendlyName;
            this.AccountName = accountName;
            this.BaseUrl = baseUrl;
#pragma warning disable 618,612
            this.InternalProjectsList = new List<string>();
#pragma warning restore 618,612
            this.LinkedTokens = new List<Guid>();

            if (tokenId != null)
            {
                this.LinkedTokens.Add(tokenId.Value);
            }

            this.InternalProjectsAndTeams = new Dictionary<string, List<string>>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AzureDevOpsAccount" /> class.
        /// </summary>
        public AzureDevOpsAccount()
        {
        }

        /// <summary>
        ///     Gets or sets the name of the account.
        /// </summary>
        /// <value>
        ///     The name of the account.
        /// </value>
        public string AccountName { get; set; }

        /// <summary>
        ///     Gets or sets the account projects.
        /// </summary>
        /// <value>
        ///     The account projects.
        /// </value>
        [JsonIgnore]
#pragma warning disable 612,618
        public IReadOnlyList<string> AccountProjects => this.InternalProjectsList;
#pragma warning restore 612,618

        /// <summary>
        ///     Gets the account projects and teams.
        /// </summary>
        /// <value>
        ///     The account projects and teams.
        /// </value>
        [JsonIgnore]
        public IReadOnlyDictionary<string, List<string>> AccountProjectsAndTeams
        {
            get
            {
                return this.InternalProjectsAndTeams;
            }
        }

        /// <summary>
        ///     Gets or sets the base URL.
        /// </summary>
        /// <value>
        ///     The base URL.
        /// </value>
        public string BaseUrl { get; set; }

        /// <summary>
        ///     Gets or sets the name of the friendly.
        /// </summary>
        /// <value>
        ///     The name of the friendly.
        /// </value>
        public string FriendlyName { get; set; }

        /// <summary>
        ///     Gets the linked pat tokens.
        /// </summary>
        /// <value>
        ///     The linked pat tokens.
        /// </value>
        [JsonIgnore]
        public IReadOnlyList<Guid> LinkedPatTokens
        {
            get
            {
                return this.LinkedTokens;
            }
        }

        /// <summary>
        ///     Gets or sets the token identifier.
        /// </summary>
        /// <value>
        ///     The token identifier.
        /// </value>
        [Obsolete(null, false)]
        public Guid? TokenId { get; set; }

        /// <summary>
        ///     Gets or sets the <see langword="internal" /> projects and teams.
        /// </summary>
        /// <value>
        ///     The <see langword="internal" /> projects and teams.
        /// </value>
        [JsonProperty(PropertyName = "AccountProjectsAndTeams")]
        private Dictionary<string, List<string>> InternalProjectsAndTeams { get; set; }

        /// <summary>
        ///     Gets or sets the <see langword="internal" /> projects list.
        /// </summary>
        /// <value>
        ///     The <see langword="internal" /> projects list.
        /// </value>
        [JsonProperty(PropertyName = "AccountProjects")]
        [Obsolete("This property has been replaced by the InternalProjectsAndTeams property")]
        private List<string> InternalProjectsList { get; set; }

        /// <summary>
        ///     Gets or sets the linked tokens.
        /// </summary>
        /// <value>
        ///     The linked tokens.
        /// </value>
        [JsonProperty("LinkedTokens")]
        private List<Guid> LinkedTokens { get; set; }

        /// <summary>
        ///     Adds the project to the account.
        /// </summary>
        /// <param name="name">The project name.</param>
        /// <exception cref="AzureDevOpsMgmt.Exceptions.ObjectExistsException">
        ///     This exception is thrown if the user tries to add a project that
        ///     already exists in the account object.
        /// </exception>
        public void AddProject(string name)
        {
            Guard.StringNotNull(nameof(name), name);
            Guard.Requires(
                           !this.InternalProjectsAndTeams.ContainsKey(name),
                           () => new ObjectExistsException("Project"));

            this.InternalProjectsAndTeams.Add(name, new List<string>());
        }

        /// <summary>
        ///     Adds the project team.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="teamName">Name of the team.</param>
        public void AddProjectTeam(string projectName, string teamName)
        {
            Guard.StringNotNull(nameof(projectName), projectName);
            Guard.StringNotNull(nameof(teamName), teamName);
            Guard.Requires<ProjectNotFoundException>(this.InternalProjectsAndTeams.ContainsKey(projectName));

            if (!this.InternalProjectsAndTeams[projectName].Contains(teamName))
            {
                this.InternalProjectsAndTeams[projectName].Add(teamName);
            }
        }

        /// <summary>
        ///     Removes the project from the account.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemoveProject(string name)
        {
            Guard.StringNotNull(nameof(name), name);
            Guard.Requires<NoProjectsFoundException>(this.InternalProjectsAndTeams.ContainsKey(name));

            this.InternalProjectsAndTeams.Remove(name);
        }

        /// <summary>
        ///     Removes the project team.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="teamName">Name of the team.</param>
        public void RemoveProjectTeam(string projectName, string teamName)
        {
            Guard.StringNotNull(nameof(projectName), projectName);
            Guard.StringNotNull(nameof(teamName), teamName);
            Guard.Requires<ProjectNotFoundException>(this.InternalProjectsAndTeams.ContainsKey(projectName));

            if (this.InternalProjectsAndTeams[projectName].Contains(teamName))
            {
                this.InternalProjectsAndTeams[projectName].Remove(teamName);
            }
        }

        /// <summary>
        ///     Adds the linked token.
        /// </summary>
        /// <param name="tokenId">The token identifier.</param>
        public void AddLinkedToken(Guid tokenId)
        {
            Guard.Requires<ArgumentNullException>(tokenId != default);
            Guard.Requires<InvalidOperationException>(
                                                      !this.LinkedTokens.Contains(tokenId),
                                                      "The Token specified is already linked to this account");

            this.LinkedTokens.Add(tokenId);
        }

        /// <summary>
        ///     Removes the linked token.
        /// </summary>
        /// <param name="tokenId">The token identifier.</param>
        // ReSharper disable once UnusedMember.Global
        public void RemoveLinkedToken(Guid tokenId)
        {
            Guard.Requires<ArgumentNullException>(tokenId != default);
            Guard.Requires<InvalidOperationException>(
                                                      this.LinkedTokens.Contains(tokenId),
                                                      "The token specified is not linked to this account");

            this.LinkedTokens.Remove(tokenId);
        }

        /// <summary>
        ///     Upgrades the project list.
        /// </summary>
        internal void UpgradeProjectList()
        {
            Guard.Requires(
                           this.InternalProjectsAndTeams == null,
                           () => new AccountProjectsAlreadyUpgradedException(this.FriendlyName));
#pragma warning disable 618
            Guard.Requires<NoProjectsFoundException>(this.InternalProjectsList.Any());
#pragma warning restore 618

            this.InternalProjectsAndTeams = new Dictionary<string, List<string>>();

#pragma warning disable 618
            foreach (var project in this.InternalProjectsList)
#pragma warning restore 618
            {
                this.InternalProjectsAndTeams.Add(project, new List<string>());
            }
        }

        /// <summary>
        ///     Migrates the token to linked tokens.
        /// </summary>
        internal void MigrateTokenToLinkedTokens()
        {
            if (this.LinkedTokens == null)
            {
                this.LinkedTokens = new List<Guid>();
            }

#pragma warning disable 618,612
            if (this.TokenId != null)
            {
                this.LinkedTokens.Add(this.TokenId.Value);
                this.TokenId = null;
            }
            #pragma warning restore 618,612
        }
    }
}