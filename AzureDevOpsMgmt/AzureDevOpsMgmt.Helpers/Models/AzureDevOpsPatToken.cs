namespace AzureDevOpsMgmt.Helpers.Models
{
    using System;

    public class AzureDevOpsPatToken
    {
        public AzureDevOpsPatToken(Guid id, string friendlyName, string tokenFileName)
        {
             
        }

        public Guid Id { get; set; }

        public string FriendlyName { get; set; }

        public string TokenFileName { get; set; }

        public Lazy<string> TokenValue { get; set; }
    }
}
