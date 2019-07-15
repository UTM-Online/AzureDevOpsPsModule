namespace AzureDevOpsMgmt.Exceptions
{
    using System;

    public class AccountProjectsAlreadyUpgradedException : Exception
    {
        public AccountProjectsAlreadyUpgradedException(string accountFriendlyName) : base($"An account with the name {accountFriendlyName} and Id of has already been upgraded!")
        {
        }
    }
}