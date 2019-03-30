// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : joirwi
// Created          : 03-25-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-25-2019
// ***********************************************************************
// <copyright file="ObjectExistsException.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
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
        /// Initializes a new instance of the <see cref="ObjectExistsException"/> class.
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
