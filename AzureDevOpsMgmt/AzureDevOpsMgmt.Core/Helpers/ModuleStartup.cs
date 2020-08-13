namespace AzureDevOpsMgmt.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using Meziantou.Framework.Win32;
    using Microsoft.TeamFoundation.Framework.Common;
    using Models;
    using Resources;

    public class ModuleStartup : IModuleAssemblyInitializer
    {
        public void OnImport()
        {
            var accountData = this.LoadAccountData();
            var configuration = this.LoadUserConfiguration();

            AzureDevOpsConfiguration.Config.Accounts = accountData;
            AzureDevOpsConfiguration.Config.Configuration = configuration;

            if ((configuration.DefaultAccount != null) & (configuration.DefaultProject != null))
            {
                AzureDevOpsAccount defaultAccount = null;
                AzureDevOpsPatToken defaultPatToken = null;

                try
                {
                    defaultAccount = accountData.Accounts.First(a => a.FriendlyName == configuration.DefaultAccount);
                    defaultPatToken = accountData.PatTokens.First(a => defaultAccount.LinkedPatTokens.Contains(a.Id));
                }
                catch (InvalidOperationException ioe) when (ioe.Message == "Sequence contains no matching element")
                {
                    this.ResetUserDefaultSettings();

                    var warningParams = new Dictionary<string, string>()
                                        {
                                            {
                                                "Message",
                                                "Corruption encountered well loading user default settings!  Settings have been reset to default (empty) values and will need to be configured again."
                                            }
                                        };
                    this.InvokePsCommand("Write-Warning", warningParams);
                }

                if (defaultAccount != null && defaultPatToken != null)
                {
                    AzureDevOpsConfiguration.Config.CurrentConnection = new CurrentConnection(defaultAccount, defaultPatToken, configuration.DefaultProject);

                    var outputParams = new Dictionary<string, string>()
                                       {
                                           {
                                               "Message",
                                               $"Default Account Settings Loaded Successfully.\r\nAccount Name: {configuration.DefaultAccount}\r\nProject Name: {configuration.DefaultProject}"
                                           }
                                       };
                    this.InvokePsCommand("Write-Host", outputParams);
                }
            }

            if (!AzureDevOpsConfiguration.Config.Accounts.HasCompleted1905Upgrade)
            {
                foreach (var token in AzureDevOpsConfiguration.Config.Accounts.PatTokens)
                {
                    ModuleStartup.ProcessCredentials(token);
                }

                AzureDevOpsConfiguration.Config.Accounts.HasCompleted1905Upgrade = true;
                FileHelpers.WriteFileJson(FileNames.AccountData, AzureDevOpsConfiguration.Config.Accounts);
            }

            var setVariableParams = new Dictionary<string,object>()
                                    {
                                        {"Name", "AzureDevOpsConfiguration"},
                                        {"Value", AzureDevOpsConfiguration.Config}
                                    };

            this.InvokePsCommand("Set-Variable", setVariableParams);
        }

        /// <summary>
        /// Processes the credentials.
        /// </summary>
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
        /// Loads the account data.
        /// </summary>
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

#pragma warning disable 618,612
                if (accountData.Accounts.Any(a => a.TokenId != null))
#pragma warning restore 618,612
                {
                    accountData = this.MigrateTokenIdToTokenList(accountData);
                }
            }

            return accountData;
        }

        /// <summary>
        /// Loads the user configuration.
        /// </summary>
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

        /// <summary>
        /// Repairs the users default settings.
        /// </summary>
        private void ResetUserDefaultSettings()
        {
            var configuration = new UserConfiguration();

            FileHelpers.WriteFileJson(FileNames.UserData, configuration);
        }

        /// <summary>
        /// Migrates the token identifier to token list.
        /// </summary>
        /// <param name="accountsCollection">The accounts collection.</param>
        /// <returns>The Azure DevOps Account Collection.</returns>
        private AzureDevOpsAccountCollection MigrateTokenIdToTokenList(AzureDevOpsAccountCollection accountsCollection)
        {
            var updatedAccounts = new List<AzureDevOpsAccount>();

#pragma warning disable 618,612
            foreach (AzureDevOpsAccount account in accountsCollection.Accounts.Where(a => a.TokenId != null))
#pragma warning restore 618,612
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