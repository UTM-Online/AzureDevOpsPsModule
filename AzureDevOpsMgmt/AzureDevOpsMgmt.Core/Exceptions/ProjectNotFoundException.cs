// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="ProjectNotFoundException.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Exceptions
{
    using System;

    /// <summary>
    /// Class ProjectNotFoundException.
    /// Implements the <see cref="System.Exception" />
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ProjectNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectNotFoundException"/> class.
        /// </summary>
        public ProjectNotFoundException() : base("The requested project was not found")
        {
        }
    }
}