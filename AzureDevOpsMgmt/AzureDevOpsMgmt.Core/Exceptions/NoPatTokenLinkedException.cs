namespace AzureDevOpsMgmt.Exceptions
{
    using System;

    public class NoPatTokenLinkedException : ApplicationException
    {
        public NoPatTokenLinkedException(string accountName) : base($"{accountName} does not have a PAT token assigned to it")
        {
        }
    }
}
