// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 05-29-2019
// ***********************************************************************
// <copyright file="ResponseModel.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Class ResponseModel.
    /// </summary>
    /// <typeparam name="T">The type stored in the response model</typeparam>
    public class ResponseModel<T>
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public List<T> Value { get; set; }
    }
}