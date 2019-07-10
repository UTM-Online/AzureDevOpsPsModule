
// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Guards
// Author           : Josh Irwin
// Created          : 05-31-2019
// ***********************************************************************
// <copyright file="Guard.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Guards
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Class Guard.
    /// </summary>
    [SuppressMessage("ReSharper", "ExceptionNotDocumented", Justification = "Exceptions not documented as they're user specified")]
    public static class Guard
    {
        /// <summary>
        /// Strings the not null.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the input <param name="value"/> is <see languageref="null"/></exception>
        public static void StringNotNull(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>Requires the specified requirement.</summary>
        /// <typeparam name="T">The type of exception to be thrown</typeparam>
        /// <param name="requirement">  if false throws the exception specified</param>
        public static void Requires<T>(bool requirement)
            where T : Exception, new()
        {
            if (!requirement)
            {
                throw new T();
            }
        }

        /// <summary>Requires the specified requirement.</summary>
        /// <typeparam name="T">The type of exception to be thrown</typeparam>
        /// <param name="requirement">  if this evaluates to false it throws an exception of type T</param>
        /// <param name="exceptionMessage">The exception message.</param>
        public static void Requires<T>(bool requirement, string exceptionMessage) where T : Exception, new()
        {
            if (!requirement)
            {
                var exception = new Exception(exceptionMessage);
                throw (T)exception;
            }
        }

        /// <summary>Requires the specified requirement.</summary>
        /// <typeparam name="T">Type of exception to be thrown</typeparam>
        /// <param name="requirement">  if evaluated to false throws an exception of type T</param>
        /// <param name="exceptionFactory">The exception factory.</param>
        public static void Requires<T>(bool requirement, Func<T> exceptionFactory) where T : Exception
        {
            if (!requirement)
            {
                var exception = exceptionFactory.Invoke();
                throw exception;
            }
        }
    }
}