// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Helpers
// Author           : joirwi
// Created          : 03-15-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-15-2019
// ***********************************************************************
// <copyright file="FileHelpers.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AzureDevOpsMgmt.Helpers
{
    using System;
    using System.IO;
    using System.Linq;

    using AzureDevOpsMgmt.Exceptions;
    using AzureDevOpsMgmt.Models;
    using AzureDevOpsMgmt.Resources;

    using Newtonsoft.Json;

    /// <summary>
    /// Class FileHelpers.
    /// </summary>
    public static class FileHelpers
    {
        /// <summary>
        /// Gets the configuration file path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The config file path.</returns>
        public static string GetConfigFilePath(string fileName = null)
        {
            return $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\WindowsPowerShell\\Modules\\{StaticStrings.ModuleName}\\Config\\{fileName}";
        }

        /// <summary>
        /// Reads the file json.
        /// </summary>
        /// <typeparam name="T">The type of the serialized object</typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The deserialized instance of T.</returns>
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file specified in <paramref name="fileName" /> was not found.</exception>
        /// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        public static T ReadFileJson<T>(string fileName)
        {
            var filePath = GetConfigFilePath(fileName);

            using (var file = File.OpenText(filePath))
            {
                return JsonConvert.DeserializeObject<T>(file.ReadToEnd());
            }
        }

        /// <summary>
        /// Writes the file json.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileData">The file data.</param>
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        public static void WriteFileJson(string fileName, object fileData)
        {
            var serializedObject = JsonConvert.SerializeObject(fileData);

            var filePath = GetConfigFilePath(fileName);

            using (var file = File.CreateText(filePath))
            {
                file.Write(serializedObject);
            }
        }

        /// <summary>Writes the file json.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileData">The file data.</param>
        /// <exception cref="EmptyIdFoundException"></exception>
        public static void WriteFileJson(string fileName, AzureDevOpsAccountCollection fileData)
        {
            if (fileData.PatTokens.Any(p => p.Id == Guid.Empty))
            {
                throw new EmptyIdFoundException(EventMessages.TOKEN_ID_CANNOT_BE_EMPTY_GUID);
            }

            WriteFileJson(fileName, fileData);
        }
    }
}
