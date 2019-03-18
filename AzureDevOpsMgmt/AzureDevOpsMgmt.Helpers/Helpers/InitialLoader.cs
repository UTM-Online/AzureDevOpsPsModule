// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Helpers
// Author           : joirwi
// Created          : 03-15-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="InitialLoader.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AzureDevOpsMgmt.Helpers.Helpers
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;

    using AzureDevOpsMgmt.Helpers.Models;
    using AzureDevOpsMgmt.Helpers.Resources;

    /// <summary>
    /// Class InitialLoader.
    /// </summary>
    public static class InitialLoader
    {
        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file specified was not found.</exception>
        /// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        public static void LoadConfiguration()
        {
            AzureDevOpsAccountCollection accountData;

            if (!File.Exists(FileHelpers.GetConfigFilePath(FileNames.AccountData)))
            {
                var dirInfo = new DirectoryInfo(FileHelpers.GetConfigFilePath(FileNames.AccountData));

                if (!dirInfo.Parent.Exists)
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
            }

            AzureDevOpsConfiguration.Config.Accounts = accountData;

            var ps = PowerShell.Create(RunspaceMode.CurrentRunspace);
            ps.Runspace.InitialSessionState.Variables.Add(new SessionStateVariableEntry("AzureDevOpsConfiguration", AzureDevOpsConfiguration.Config, null));
        }
    }
}
