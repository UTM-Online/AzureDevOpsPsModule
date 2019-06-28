namespace AzureDevOpsMgmt.Models.Contracts
{
    using System;

    public interface IPortable
    {
        Guid? MachineScopeId { get; set; }

        bool IsInScope { get; }
    }
}