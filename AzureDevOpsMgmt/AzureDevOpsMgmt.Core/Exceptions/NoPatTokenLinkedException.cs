// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 05-29-2019
// ***********************************************************************
// <copyright file="NoPatTokenLinkedException.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Exceptions
{
    using System;

    /// <summary>
    /// Class NoPatTokenLinkedException.
    /// Implements the <see cref="System.ApplicationException" />
    /// </summary>
    /// <seealso cref="System.ApplicationException" />
    public class NoPatTokenLinkedException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoPatTokenLinkedException"/> class.
        /// </summary>
        /// <param name="accountName">Name of the account.</param>
        public NoPatTokenLinkedException(string accountName) : base($"{accountName} does not have a PAT token assigned to it")
        {
        }
    }
}
