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
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;
    using AzureDevOpsMgmt.Resources;

    using Meziantou.Framework.Win32;

    /// <summary>
    ///     Class ImportConfiguration.
    ///     Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    [Cmdlet(VerbsData.Import, "Configuration")]
    public class ImportConfiguration : PSCmdlet
    {
        /// <summary>Processes the credentials.</summary>
        /// <param name="token">The token.</param>
        internal static void ProcessCredentials(AzureDevOpsPatToken token)
        {
            if (token.MachineScopeId == Guid.Empty
                && !token.NotOnMachines.Contains(ConfigurationHelpers.GetMachineId()))
            {
                var cred = CredentialManager.ReadCredential(token.CredentialManagerId);

                if (cred != default(Credential))
                {
                    token.MachineScopeId = ConfigurationHelpers.GetMachineId();
                }
                else
                {
                    token.NotOnMachines.Add(ConfigurationHelpers.GetMachineId());
                }
            }
        }

        /// <summary>
        ///     When overridden in the derived class, performs execution
        ///     of the command.
        /// </summary>
        protected override void ProcessRecord()
        {
            var accountData = this.LoadAccountData();
            var configuration = this.LoadUserConfiguration();

            AzureDevOpsConfiguration.Config.Accounts = accountData;
            AzureDevOpsConfiguration.Config.Configuration = configuration;

            if ((configuration.DefaultAccount != null) & (configuration.DefaultProject != null))
            {
                var defaultAccount = accountData.Accounts.First(a => a.FriendlyName == configuration.DefaultAccount);
                var defaultPatToken = accountData.PatTokens.First(a => a.Id == defaultAccount.TokenId);
                AzureDevOpsConfiguration.Config.CurrentConnection = new CurrentConnection(
                    defaultAccount,
                    defaultPatToken,
                    configuration.DefaultProject);
                this.WriteObject(
                    $"Default Account Settings Loaded Successfully.\r\nAccount Name: {configuration.DefaultAccount}\r\nProject Name: {configuration.DefaultProject}");
            }

            if (!AzureDevOpsConfiguration.Config.Accounts.HasCompleted1905Upgrade)
            {
                foreach (var token in AzureDevOpsConfiguration.Config.Accounts.PatTokens)
                {
                    ImportConfiguration.ProcessCredentials(token);
                }

                AzureDevOpsConfiguration.Config.Accounts.HasCompleted1905Upgrade = true;
                FileHelpers.WriteFileJson(FileNames.AccountData, AzureDevOpsConfiguration.Config.Accounts);
            }

            this.SetPsVariable("AzureDevOpsConfiguration", AzureDevOpsConfiguration.Config);
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

                accountData = new AzureDevOpsAccountCollection
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
            }

            return accountData;
        }

        /// <summary>Loads the user configuration.</summary>
        /// <returns>The UserConfiguration.</returns>
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
    }
}