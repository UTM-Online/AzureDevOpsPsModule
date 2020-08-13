// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 06-07-2019
// ***********************************************************************
// <copyright file="OperationCheck.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************

namespace AzureDevOpsMgmt.Helpers
{
    using System;

    using AzureDevOpsMgmt.Models;

    /// <summary>
    /// Class OperationCheck.
    /// </summary>
    public class OperationCheck
    {
        /// <summary>
        /// Throws if connection not set.
        /// </summary>
        /// <exception cref="InvalidOperationException">Is thrown when no Azure Dev Ops Account / Project Context is set for the modules execution</exception>
        public void ThrowIfConnectionNotSet()
        {
            if (!AzureDevOpsConfiguration.Config.ReadyForCommands)
            {
                throw new InvalidOperationException("Please run the \"Set-AzureDevOpsAccount\" cmdlet to set the current account context");
            }
        }
    }
}
