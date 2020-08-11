namespace AzureDevOpsMgmt.Cmdlets
{
    using System.Collections;
    using System.Collections.Generic;

    public class InvokeAdoRestApi : ApiCmdlet
    {
        public string ResourcePath { get; set; }

        public List<KeyValuePair<string,string>> QueryParameters { get; set; }

        
    }
}