// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 04-25-2019
// ***********************************************************************
// <copyright file="JsonPatchOperationContractResolver.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Serialization
{
    using System;
    using System.Reflection;

    using Microsoft.VisualStudio.Services.WebApi.Patch;
    using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Class JsonPatchOperationContractResolver.
    /// Implements the <see cref="Newtonsoft.Json.Serialization.DefaultContractResolver" />
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.Serialization.DefaultContractResolver" />
    public class JsonPatchOperationContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// The instance
        /// </summary>
        public static readonly JsonPatchOperationContractResolver Instance = new JsonPatchOperationContractResolver();

        /// <summary>
        /// Creates a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given <see cref="T:System.Reflection.MemberInfo" />.
        /// </summary>
        /// <param name="member">The member to create a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for.</param>
        /// <param name="memberSerialization">The member's parent <see cref="T:Newtonsoft.Json.MemberSerialization" />.</param>
        /// <returns>A created <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given <see cref="T:System.Reflection.MemberInfo" />.</returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(JsonPatchOperation))
            {
                if (property.PropertyName.Equals("Operation", StringComparison.OrdinalIgnoreCase))
                {
                    property.PropertyName = "op";
                }
                else
                {
                    property.PropertyName = property.PropertyName.ToLower();
                }

                if (property.PropertyName == "From")
                {
                    property.ShouldSerialize = instance =>
                        {
                            var op = (JsonPatchOperation)instance;
                            return op.Operation == Operation.Copy || op.Operation == Operation.Move;
                        };
                }

                if (property.PropertyName == "Value")
                {
                    property.ShouldSerialize = instance =>
                        {
                            var op = (JsonPatchOperation)instance;
                            return op.Operation == Operation.Add || op.Operation == Operation.Replace;
                        };
                }
            }

            return property;
        }
    }
}
