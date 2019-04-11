namespace AzureDevOpsMgmt.Models
{
    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Resources;

    public class UserConfiguration
    {
        public string DefaultAccount { get; set; }

        public string DefaultProject { get; set; }

        public void Save()
        {
            FileHelpers.WriteFileJson(FileNames.UserData, this);
        }
    }
}