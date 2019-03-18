// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Helpers
// Author           : joirwi
// Created          : 03-18-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-18-2019
// ***********************************************************************
// <copyright file="OperationCheck.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace AzureDevOpsMgmt.Helpers.Helpers
{
    using System;

    using AzureDevOpsMgmt.Helpers.Models;

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
            if (!AzureDevOpsConfiguration.ReadyForCommands)
            {
                throw new InvalidOperationException("Please run the \"Set-AzureDevOpsAccount\" cmdlet to set the current account context");
            }
        }
    }
}
