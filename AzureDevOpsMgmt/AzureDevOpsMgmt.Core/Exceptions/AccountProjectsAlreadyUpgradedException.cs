// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="AccountProjectsAlreadyUpgradedException.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Exceptions
{
    using System;

    /// <summary>
    /// Class AccountProjectsAlreadyUpgradedException.
    /// Implements the <see cref="System.Exception" />
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class AccountProjectsAlreadyUpgradedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountProjectsAlreadyUpgradedException"/> class.
        /// </summary>
        /// <param name="accountFriendlyName">Name of the account friendly.</param>
        public AccountProjectsAlreadyUpgradedException(string accountFriendlyName) : base($"An account with the name {accountFriendlyName} and Id of has already been upgraded!")
        {
        }
    }
}