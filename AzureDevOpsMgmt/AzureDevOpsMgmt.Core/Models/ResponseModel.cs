namespace AzureDevOpsMgmt.Models
{
    using System.Collections.Generic;

    public class ResponseModel<T>
    {
        public int Count { get; set; }

        public List<T> Value { get; set; }
    }
}