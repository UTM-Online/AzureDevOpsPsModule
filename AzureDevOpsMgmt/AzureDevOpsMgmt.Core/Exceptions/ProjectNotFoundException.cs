namespace AzureDevOpsMgmt.Exceptions
{
    using System;

    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException() : base("The requested project was not found")
        {
        }
    }
}