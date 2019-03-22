namespace AzureDevOpsMgmt.Serialization
{
    using System;
    using System.Reflection;

    using Microsoft.VisualStudio.Services.WebApi.Patch;
    using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class JsonPatchOperationContractResolver : DefaultContractResolver
    {
        public static readonly JsonPatchOperationContractResolver Instance = new JsonPatchOperationContractResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (property.DeclaringType == typeof(JsonPatchOperation))
            {
                if (property.PropertyName.Equals("Operation", StringComparison.OrdinalIgnoreCase))
                {
                    property.PropertyName = "op";
                }
                else
                {
                    property.PropertyName = property.PropertyName.ToLower();
                }

                if (property.PropertyName == "From")
                {
                    property.ShouldSerialize = instance =>
                        {
                            var op = (JsonPatchOperation)instance;
                            return op.Operation == Operation.Copy || op.Operation == Operation.Move;
                        };
                }

                if (property.PropertyName == "Value")
                {
                    property.ShouldSerialize = instance =>
                        {
                            var op = (JsonPatchOperation)instance;
                            return op.Operation == Operation.Add || op.Operation == Operation.Replace;
                        };
                }
            }

            return property;
        }
    }
}
