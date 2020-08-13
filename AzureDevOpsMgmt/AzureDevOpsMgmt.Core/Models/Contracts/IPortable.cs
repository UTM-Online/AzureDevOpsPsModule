// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="IPortable.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Models.Contracts
{
    using System;

    /// <summary>
    /// Interface IPortable
    /// </summary>
    public interface IPortable
    {
        /// <summary>
        /// Gets or sets the machine scope identifier.
        /// </summary>
        /// <value>The machine scope identifier.</value>
        Guid? MachineScopeId { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is in scope.
        /// </summary>
        /// <value><c>true</c> if this instance is in scope; otherwise, <c>false</c>.</value>
        bool IsInScope { get; }
    }
}