// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Helpers
// Author           : joirwi
// Created          : 03-19-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-19-2019
// ***********************************************************************
// <copyright file="ImportConfiguration.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace AzureDevOpsMgmt.Cmdlets.Startup
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;
    using AzureDevOpsMgmt.Resources;

    /// <summary>
    /// Class ImportConfiguration.
    /// Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    [Cmdlet(VerbsData.Import, "Configuration")]
    public class ImportConfiguration : PSCmdlet
    {
        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.IO.IOException">The directory cannot be created.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The configuration file specified was not found.</exception>
        /// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
        /// <exception cref="T:System.Management.Automation.ProviderNotFoundException">If the Configuration refers to a provider that could not be found.</exception>
        /// <exception cref="T:System.Management.Automation.DriveNotFoundException">If the Configuration refers to a drive that could not be found.</exception>
        /// <exception cref="T:System.Management.Automation.ProviderInvocationException">If the provider threw an exception.</exception>
        /// <exception cref="T:System.Management.Automation.SessionStateUnauthorizedAccessException">If the variable is read-only or constant.</exception>
        /// <exception cref="T:System.Management.Automation.SessionStateOverflowException">If the maximum number of variables has been reached for this scope.</exception>
        protected override void ProcessRecord()
        {
            var accountData = this.LoadAccountData();
            var configuration = this.LoadUserConfiguration();

            AzureDevOpsConfiguration.Config.Accounts = accountData;
            AzureDevOpsConfiguration.Config.Configuration = configuration;

            if (configuration.DefaultAccount != null & configuration.DefaultProject != null)
            {
                AzureDevOpsAccount defaultAccount = null;
                AzureDevOpsPatToken defaultPatToken = null;

                try
                {
                    defaultAccount = accountData.Accounts.First(a => a.FriendlyName == configuration.DefaultAccount);
                    defaultPatToken = accountData.PatTokens.First(a => defaultAccount.LinkedPatTokens.Contains(a.Id));
                }
                catch (InvalidOperationException ioe) when(ioe.Message == "Sequence contains no matching element")
                {
                    this.ResetUserDefaultSettings();
                    this.WriteWarning("Corruption encountered well loading user default settings!  Settings have been reset to default (empty) values and will need to be configured again.");
                }

                if (defaultAccount != null && defaultPatToken != null)
                {
                    AzureDevOpsConfiguration.Config.CurrentConnection = new CurrentConnection(defaultAccount, defaultPatToken, configuration.DefaultProject);
                    this.WriteObject($"Default Account Settings Loaded Successfully.\r\nAccount Name: {configuration.DefaultAccount}\r\nProject Name: {configuration.DefaultProject}");
                }
            }

            this.SetPsVariable("AzureDevOpsConfiguration", AzureDevOpsConfiguration.Config);
        }

        private UserConfiguration LoadUserConfiguration()
        {
            UserConfiguration configuration;

            var filePath = FileHelpers.GetConfigFilePath(FileNames.UserData);

            if (!File.Exists(filePath))
            {
                var dirinfo = new DirectoryInfo(filePath);

                if (dirinfo.Parent != null && !dirinfo.Parent.Exists)
                {
                    dirinfo.Parent.Create();
                }

                configuration = new UserConfiguration();

                FileHelpers.WriteFileJson(FileNames.UserData, configuration);
            }
            else
            {
                configuration = FileHelpers.ReadFileJson<UserConfiguration>(FileNames.UserData);
            }

            return configuration;
        }

        /// <summary>Loads the account data.</summary>
        /// <returns>Account configuration collection.</returns>
        private AzureDevOpsAccountCollection LoadAccountData()
        {
            AzureDevOpsAccountCollection accountData;

            if (!File.Exists(FileHelpers.GetConfigFilePath(FileNames.AccountData)))
            {
                var dirInfo = new DirectoryInfo(FileHelpers.GetConfigFilePath(FileNames.AccountData));

                if (dirInfo.Parent != null && !dirInfo.Parent.Exists)
                {
                    dirInfo.Parent.Create();
                }

                accountData = new AzureDevOpsAccountCollection()
                                  {
                                      Accounts = new ObservableCollection<AzureDevOpsAccount>(),
                                      PatTokens = new ObservableCollection<AzureDevOpsPatToken>()
                                  };

                accountData.Init();

                FileHelpers.WriteFileJson(FileNames.AccountData, accountData);
            }
            else
            {
                accountData = FileHelpers.ReadFileJson<AzureDevOpsAccountCollection>(FileNames.AccountData);
                accountData.Init();

                if (accountData.Accounts.Any(a => a.TokenId != null))
                {
                    accountData = this.MigrateTokenIdToTokenList(accountData);
                }
            }

            if (accountData.PatTokens.Any(p => p.Id == Guid.Empty))
            {
                this.WriteWarning("A PAT Token was imported that contained an empty GUID.  Interactions with Azure Dev Ops may not work as expected.");
            }

            return accountData;
        }

        /// <summary>Repairs the users default settings.</summary>
        private void ResetUserDefaultSettings()
        {
            var configuration = new UserConfiguration();

            FileHelpers.WriteFileJson(FileNames.UserData, configuration);
        }

        /// <summary>Migrates the token identifier to token list.</summary>
        /// <param name="accountsCollection">The accounts collection.</param>
        /// <returns>The Azure DevOps Account Collection.</returns>
        private AzureDevOpsAccountCollection MigrateTokenIdToTokenList(AzureDevOpsAccountCollection accountsCollection)
        {
            var updatedAccounts = new List<AzureDevOpsAccount>();

            foreach (AzureDevOpsAccount account in accountsCollection.Accounts.Where(a => a.TokenId != null))
            {
                account.MigrateTokenToLinkedTokens();

                updatedAccounts.Add(account);
            }

            if (updatedAccounts.Any())
            {
                foreach (var account in updatedAccounts)
                {
                    accountsCollection.PerformAccountUpdate(account);
                }
            }

            return accountsCollection;
        }
    }
}
