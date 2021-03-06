﻿// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="JsonNetSerializer.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************

namespace AzureDevOpsMgmt.Serialization
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using RestSharp;
    using RestSharp.Serialization;

    /// <summary>
    /// Class JsonNetSerializer.
    /// Implements the <see cref="RestSharp.Serialization.IRestSerializer" />
    /// </summary>
    /// <seealso cref="RestSharp.Serialization.IRestSerializer" />
    public class JsonNetSerializer : IRestSerializer
    {
        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; set; } = "application/json";

        /// <summary>
        /// Gets the data format.
        /// </summary>
        /// <value>The data format.</value>
        public DataFormat DataFormat { get; } = DataFormat.Json;

        /// <summary>
        /// Gets the supported content types.
        /// </summary>
        /// <value>The supported content types.</value>
        public string[] SupportedContentTypes { get; } = { "application/json", "text/json", "text/x-json", "text/javascript", "*+json" };

        /// <summary>
        /// Deserializes the specified response.
        /// </summary>
        /// <typeparam name="T">The type being deserialized from the response</typeparam>
        /// <param name="response">The response.</param>
        /// <returns>The object returned in the response</returns>
        public T Deserialize<T>(IRestResponse response) =>
            JsonConvert.DeserializeObject<T>(response.Content, this.GetSettings());

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The input object serialized to a string.</returns>
        public string Serialize(object obj) => JsonConvert.SerializeObject(obj, this.GetSettings());

        /// <summary>
        /// Serializes the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The serialized parameter.</returns>
        public string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter.Value, this.GetSettings());

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <returns>The Json Serializer Settings.</returns>
        private JsonSerializerSettings GetSettings()
        {
            var settings = new JsonSerializerSettings
                               {
                                   FloatFormatHandling = FloatFormatHandling.DefaultValue,
                                   FloatParseHandling = FloatParseHandling.Double,
                                   ContractResolver = JsonPatchOperationContractResolver.Instance,
                                   NullValueHandling = NullValueHandling.Ignore
                               };

            settings.Converters.Add(new StringEnumConverter { AllowIntegerValues = false });

            return settings;
        }
    }
}