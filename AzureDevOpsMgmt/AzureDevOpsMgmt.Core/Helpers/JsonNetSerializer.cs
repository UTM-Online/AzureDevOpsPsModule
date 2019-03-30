// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : joirwi
// Created          : 03-19-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-19-2019
// ***********************************************************************
// <copyright file="JsonNetSerializer.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace AzureDevOpsMgmt.Serialization
{
    using Newtonsoft.Json;

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
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.String.</returns>
        public string Serialize(object obj) =>
            JsonConvert.SerializeObject(obj, this.GetSettings());

        /// <summary>
        /// Deserializes the specified response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">The response.</param>
        /// <returns>T.</returns>
        public T Deserialize<T>(IRestResponse response) =>
            JsonConvert.DeserializeObject<T>(response.Content);

        /// <summary>
        /// Serializes the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>System.String.</returns>
        public string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter.Value);

        /// <summary>
        /// Gets the supported content types.
        /// </summary>
        /// <value>The supported content types.</value>
        public string[] SupportedContentTypes { get; } =
        {
                "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
            };

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

        private JsonSerializerSettings GetSettings()
        {
            return new JsonSerializerSettings
                       {
                           FloatFormatHandling = FloatFormatHandling.DefaultValue,
                           FloatParseHandling = FloatParseHandling.Double
                       };
        }
    }
}
