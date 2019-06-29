// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 06-07-2019
// ***********************************************************************
// <copyright file="UserConfiguration.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Models
{
    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Resources;

    /// <summary>
    /// Class UserConfiguration.
    /// </summary>
    public class UserConfiguration
    {
        /// <summary>
        /// Gets or sets the default account.
        /// </summary>
        /// <value>The default account.</value>
        public string DefaultAccount { get; set; }

        /// <summary>
        /// Gets or sets the default project.
        /// </summary>
        /// <value>The default project.</value>
        public string DefaultProject { get; set; }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            // ReSharper disable once ExceptionNotDocumented
            FileHelpers.WriteFileJson(FileNames.UserData, this);
        }
    }
}