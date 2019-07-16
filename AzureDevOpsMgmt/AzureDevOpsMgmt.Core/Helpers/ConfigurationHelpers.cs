namespace AzureDevOpsMgmt.Helpers
{
    using System;
    using System.Diagnostics.Contracts;

    using AzureDevOpsMgmt.Models;
    using AzureDevOpsMgmt.Resources;

    using Microsoft.Win32;

    using UTMO.Common.Guards;

    public static class ConfigurationHelpers
    {
        static ConfigurationHelpers()
        {
            MachineGuid = new Lazy<Guid>(GetLocalMachineId);
        }

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

        private static Lazy<Guid> MachineGuid;

        public static bool CheckIfMachineIsInScope(this AzureDevOpsPatToken portable)
        {
            if (portable.CreatedOnMachine == default)
            {
                return false;
            }

            return portable.CreatedOnMachine == MachineGuid.Value;
        }

        public static Guid GetMachineId(this AzureDevOpsPatToken _)
        {
            return MachineGuid.Value;
        }

        public static Guid GetMachineId()
        {
            return MachineGuid.Value;
        }
    }
}