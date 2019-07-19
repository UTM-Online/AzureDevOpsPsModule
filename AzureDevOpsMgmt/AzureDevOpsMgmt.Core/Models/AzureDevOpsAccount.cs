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
            this.TokenId = tokenId;
            this.BaseUrl = baseUrl;
            this.InternalProjectsAndTeams = new Dictionary<string, List<string>>();
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
        #pragma warning disable 612,618
        public IReadOnlyList<string> AccountProjects => this.InternalProjectsList;
#pragma warning restore 612,618

        /// <summary>Gets the account projects and teams.</summary>
        /// <value>The account projects and teams.</value>
        [JsonIgnore]
        public IReadOnlyDictionary<string, List<string>> AccountProjectsAndTeams => this.InternalProjectsAndTeams;

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
        public Guid? TokenId { get; set; }

        /// <summary>Gets or sets the linked tokens.</summary>
        /// <value>The linked tokens.</value>
        public List<Guid> LinkedTokens { get; set; }

        /// <summary>Gets or sets the internal projects list.</summary>
        /// <value>The internal projects list.</value>
        [JsonProperty(PropertyName = "AccountProjects")]
        [Obsolete("This property has been replaced by the InternalProjectsAndTeams property")]
        private List<string> InternalProjectsList { get; set; }

        /// <summary>Gets or sets the internal projects and teams.</summary>
        /// <value>The internal projects and teams.</value>
        [JsonProperty(PropertyName = "AccountProjectsAndTeams")]
        private Dictionary<string, List<string>> InternalProjectsAndTeams { get; set; }

        /// <summary>Adds the project to the account.</summary>
        /// <param name="name">The project name.</param>
        /// <exception cref="T:AzureDevOpsMgmt.Exceptions.ObjectExistsException">
        ///     This exception is thrown if the user tries to add a project that already exists in the account object.
        /// </exception>
        public void AddProject(string name)
        {
            Guard.StringNotNull(nameof(name), name);
            Guard.Requires(!this.InternalProjectsAndTeams.ContainsKey(name), () => new ObjectExistsException("Project"));

            this.InternalProjectsAndTeams.Add(name, new List<string>());
        }

        /// <summary>Adds the project team.</summary>
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

        /// <summary>Removes the project team.</summary>
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

        /// <summary>Removes the project from the account.</summary>
        /// <param name="name">The name.</param>
        public void RemoveProject(string name)
        {
            Guard.StringNotNull(nameof(name), name);
            Guard.Requires<NoProjectsFoundException>(this.InternalProjectsAndTeams.ContainsKey(name));

            this.InternalProjectsAndTeams.Remove(name);
        }

#pragma warning disable 612,618
        /// <summary>Upgrades the project list.</summary>
        internal void UpgradeProjectList()
        {
            Guard.Requires(this.InternalProjectsAndTeams == null, () => new AccountProjectsAlreadyUpgradedException(this.FriendlyName));
            Guard.Requires<NoProjectsFoundException>(this.InternalProjectsList.Any());

            this.InternalProjectsAndTeams = new Dictionary<string, List<string>>();

            foreach (var project in this.InternalProjectsList)
            {
                this.InternalProjectsAndTeams.Add(project, new List<string>());
            }
        }

        #pragma warning restore 612,618
    }
}
