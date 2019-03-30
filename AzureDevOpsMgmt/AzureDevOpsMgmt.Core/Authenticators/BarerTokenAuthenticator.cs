// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : joirwi
// Created          : 03-19-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-19-2019
// ***********************************************************************
// <copyright file="BarrerTokenAuthenticator.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace AzureDevOpsMgmt.Authenticators
{
    using System;
    using System.Linq;

    using AzureDevOpsMgmt.Models;

    using RestSharp;
    using RestSharp.Authenticators;

    /// <summary>
    /// Class BarerTokenAuthenticator.
    /// Implements the <see cref="RestSharp.Authenticators.IAuthenticator" />
    /// </summary>
    /// <seealso cref="RestSharp.Authenticators.IAuthenticator" />
    public class BarerTokenAuthenticator : IAuthenticator
    {
        /// <summary>
        /// Authenticates the specified client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="T:System.MemberAccessException">The <see cref="T:System.Lazy`1" /> instance is initialized to use the default constructor of the type that is being lazily initialized, and permissions to access the constructor are missing.</exception>
        /// <exception cref="T:System.MissingMemberException">The <see cref="T:System.Lazy`1" /> instance is initialized to use the default constructor of the type that is being lazily initialized, and that type does not have a public, parameterless constructor.</exception>
        public void Authenticate(IRestClient client, IRestRequest request)
        {
            if (!request.Parameters.Any(p => "Authorization".Equals(p.Name, StringComparison.OrdinalIgnoreCase)))
            {
                request.AddHeader("Authorization", $"Basic {AzureDevOpsConfiguration.Config.CurrentConnection.Token.TokenValue.Value}");
            }
        }
    }
}
