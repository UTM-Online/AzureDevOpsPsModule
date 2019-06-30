// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : josh
// Created          : 06-28-2019
//
// Last Modified By : josh
// Last Modified On : 06-28-2019
// ***********************************************************************
// <copyright file="ConfigurationHelpers.cs" company="UTM-Online">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AzureDevOpsMgmt.Helpers
{
    using System;
    using System.Diagnostics.Contracts;

    using AzureDevOpsMgmt.Models.Contracts;
    using AzureDevOpsMgmt.Resources;

    using Microsoft.Win32;

    using UTMO.Common.Guards;

    /// <summary>
    ///     Class ConfigurationHelpers.
    /// </summary>
    public static class ConfigurationHelpers
    {
        /// <summary>
        ///     The machine unique identifier
        /// </summary>
        private static readonly Lazy<Guid> MachineGuid;

        /// <summary>
        ///     Initializes static members of the <see cref="ConfigurationHelpers" /> class.
        /// </summary>
        static ConfigurationHelpers()
        {
            ConfigurationHelpers.MachineGuid = new Lazy<Guid>(ConfigurationHelpers.GetLocalMachineId);
        }

        /// <summary>
        ///     Checks if machine is in scope.
        /// </summary>
        /// <param name="portable">The portable.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool CheckIfMachineIsInScope(this IPortable portable) =>
            portable.MachineScopeId == ConfigurationHelpers.MachineGuid.Value;

        /// <summary>
        ///     Gets the machine identifier.
        /// </summary>
        /// <param name="_">The throw away parameter.</param>
        /// <returns>The Machine Guid.</returns>
        public static Guid GetMachineId(this IPortable _) => ConfigurationHelpers.MachineGuid.Value;

        /// <summary>
        ///     Gets the machine identifier.
        /// </summary>
        /// <returns>The Machine Guid.</returns>
        public static Guid GetMachineId() => ConfigurationHelpers.MachineGuid.Value;

        /// <summary>
        ///     Gets the local machine identifier.
        /// </summary>
        /// <returns>The Machine Guid.</returns>
        private static Guid GetLocalMachineId()
        {
            var regPath = @"SOFTWARE\Microsoft\Cryptography";
            var regValue = "MachineGuid";

            string machineIdStringX64;
            string machineIdStringX86;

            using (var keyBase = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var key = keyBase.OpenSubKey(regPath, RegistryKeyPermissionCheck.ReadSubTree))
            {
                Guard.Requires<ArgumentNullException>(key != null, nameof(key));
                Contract.Assume(key != null);

                var resultObject = key.GetValue(regValue, StaticStrings.Default);
                key.Close();
                keyBase.Close();
                machineIdStringX64 = resultObject.ToString();
            }

            using (var keyBase = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            using (var key = keyBase.OpenSubKey(regPath, RegistryKeyPermissionCheck.ReadSubTree))
            {
                Guard.Requires<ArgumentNullException>(key != null, nameof(key));
                Contract.Assume(key != null);

                var resultObject = key.GetValue(regValue, StaticStrings.Default);
                key.Close();
                keyBase.Close();
                machineIdStringX86 = resultObject.ToString();
            }

            return Guid.Parse(machineIdStringX64 != StaticStrings.Default ? machineIdStringX64 : machineIdStringX86);
        }
    }
}