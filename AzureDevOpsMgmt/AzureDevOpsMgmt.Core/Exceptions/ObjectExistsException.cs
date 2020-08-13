// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 05-29-2019
// ***********************************************************************
// <copyright file="ObjectExistsException.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************

namespace AzureDevOpsMgmt.Exceptions
{
    using System;

    /// <summary>
    /// Class ObjectExistsException.
    /// Implements the <see cref="System.ApplicationException" />
    /// </summary>
    /// <seealso cref="System.ApplicationException" />
    public class ObjectExistsException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectExistsException" /> class.
        /// </summary>
        /// <param name="objectTypeName">Name of the object type.</param>
        public ObjectExistsException(string objectTypeName)
            : base($"An object of type {objectTypeName} already exists in the account repository.")
        {
            this.ObjectTypeName = objectTypeName;
        }

        /// <summary>
        /// Gets the name of the object type.
        /// </summary>
        /// <value>The name of the object type.</value>
        public string ObjectTypeName { get; }
    }
}
