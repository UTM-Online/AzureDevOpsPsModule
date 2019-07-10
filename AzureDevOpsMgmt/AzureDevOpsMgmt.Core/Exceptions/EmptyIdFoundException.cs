// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : josh
// Created          : 06-10-2019
//
// Last Modified By : josh
// Last Modified On : 06-10-2019
// ***********************************************************************
// <copyright file="EmptyIdFoundException.cs" company="UTM-Online">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace AzureDevOpsMgmt.Exceptions
{
    using System;

    /// <summary>
    /// Class EmptyIdFoundException.
    /// Implements the <see cref="System.Exception" />
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class EmptyIdFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyIdFoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EmptyIdFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyIdFoundException"/> class.
        /// </summary>
        public EmptyIdFoundException()
        {
        }
    }
}