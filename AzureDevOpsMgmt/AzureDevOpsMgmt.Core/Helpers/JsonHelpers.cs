﻿// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="JsonHelpers.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************

namespace AzureDevOpsMgmt.Helpers
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.VisualStudio.Services.WebApi.Patch;
    using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Class Json Helpers.
    /// </summary>
    // ReSharper disable once StyleCop.SA1650
    public static class JsonHelpers
    {
        /// <summary>
        /// The field name fixer
        /// </summary>
        private static readonly Regex FieldNameFixer = new Regex("/Fields/", RegexOptions.Compiled);

        /// <summary>
        /// Creates the patch.
        /// </summary>
        /// <param name="originalObject">The original object.</param>
        /// <param name="modifiedObject">The modified object.</param>
        /// <returns>The Patch Document.</returns>
        public static JsonPatchDocument CreatePatch(object originalObject, object modifiedObject)
        {
            var original = JObject.FromObject(originalObject, JsonHelpers.GetPrivateSerializer());
            var modified = JObject.FromObject(modifiedObject, JsonHelpers.GetPrivateSerializer());

            var patch = new JsonPatchDocument();
            JsonHelpers.FillPatchForObject(original, modified, patch, "/");

            return patch;
        }

        /// <summary>
        /// Adds the specified path.
        /// </summary>
        /// <param name="patchDoc">The patch document.</param>
        /// <param name="path">The path.</param>
        /// <param name="value">The value.</param>
        /// <returns>A JsonPatchDocument.</returns>
        public static JsonPatchDocument Add(this JsonPatchDocument patchDoc, string path, object value)
        {
            var patch = new JsonPatchOperation
            {
                Operation = Operation.Add, Path = JsonHelpers.SanitizePath(path), Value = value
            };
            patchDoc.Add(patch);

            return patchDoc;
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="path">The path.</param>
        /// <param name="value">The value.</param>
        /// <returns>A JsonPatchDocument.</returns>
        public static JsonPatchDocument AddIfNotNull(this JsonPatchDocument document, string path, object value)
        {
            if (value == null)
            {
                return document;
            }

            document.Add(path, value);

            return document;
        }

        /// <summary>
        /// Removes the specified path.
        /// </summary>
        /// <param name="patchDoc">The patch document.</param>
        /// <param name="path">The path.</param>
        public static void Remove(this JsonPatchDocument patchDoc, string path)
        {
            var patch = new JsonPatchOperation { Operation = Operation.Remove, Path = JsonHelpers.SanitizePath(path) };
            patchDoc.Add(patch);
        }

        /// <summary>
        /// Replaces the specified path.
        /// </summary>
        /// <param name="patchDoc">The patch document.</param>
        /// <param name="path">The path.</param>
        /// <param name="value">The value.</param>
        public static void Replace(this JsonPatchDocument patchDoc, string path, object value)
        {
            var patch = new JsonPatchOperation
            {
                Operation = Operation.Replace,
                Path = JsonHelpers.SanitizePath(path),
                Value = value
            };
            patchDoc.Add(patch);
        }

        /// <summary>
        /// Fills the patch for object.
        /// </summary>
        /// <param name="orig">The original.</param>
        /// <param name="mod">The mod.</param>
        /// <param name="patch">The patch.</param>
        /// <param name="path">The path.</param>
        private static void FillPatchForObject(JObject orig, JObject mod, JsonPatchDocument patch, string path)
        {
            var origNames = orig.Properties().Select(x => x.Name).ToArray();
            var modNames = mod.Properties().Select(x => x.Name).ToArray();

            // Names removed in modified
            foreach (var k in origNames.Except(modNames))
            {
                var prop = orig.Property(k);
                patch.Remove(path + prop.Name);
            }

            // Names added in modified
            foreach (var k in modNames.Except(origNames))
            {
                var prop = mod.Property(k);
                patch.Add(path + prop.Name, prop.Value);
            }

            // Present in both
            foreach (var k in origNames.Intersect(modNames))
            {
                var origProp = orig.Property(k);
                var modProp = mod.Property(k);

                if (origProp.Value.Type != modProp.Value.Type)
                {
                    patch.Replace(path + modProp.Name, modProp.Value.ToString());
                }
                else if (!string.Equals(
                    origProp.Value.ToString(Formatting.None),
                    modProp.Value.ToString(Formatting.None)))
                {
                    if (origProp.Value.Type == JTokenType.Object)
                    {
                        // Recurse into objects
                        JsonHelpers.FillPatchForObject(
                            origProp.Value as JObject,
                            modProp.Value as JObject,
                            patch,
                            path + modProp.Name + "/");
                    }
                    else
                    {
                        if (origProp.Value.Type == JTokenType.Float)
                        {
                            patch.Replace(path + modProp.Name, (double)modProp.Value);
                        }
                        else
                        {
                            // Replace values directly
                            patch.Replace(path + modProp.Name, modProp.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the private serializer.
        /// </summary>
        /// <returns>A JsonSerializer.</returns>
        private static JsonSerializer GetPrivateSerializer() =>
            JsonSerializer.Create(
                new JsonSerializerSettings
                {
                    FloatParseHandling = FloatParseHandling.Double,
                    FloatFormatHandling = FloatFormatHandling.DefaultValue,
                    NullValueHandling = NullValueHandling.Include,
                    Formatting = Formatting.Indented
                });

        /// <summary>
        /// Sanitizes the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The Sanitized String</returns>
        private static string SanitizePath(string path)
        {
            if (JsonHelpers.FieldNameFixer.IsMatch(path))
            {
                path = JsonHelpers.FieldNameFixer.Replace(path, "/fields/");
            }

            return path;
        }
    }
}