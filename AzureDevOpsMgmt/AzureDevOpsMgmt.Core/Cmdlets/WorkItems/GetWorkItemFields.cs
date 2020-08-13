// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 06-26-2019
// ***********************************************************************
// <copyright file="GetWorkItemFields.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************

namespace AzureDevOpsMgmt.Cmdlets.WorkItems
{
    using System.Management.Automation;

    using AzureDevOpsMgmt.Models;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    using RestSharp;

    /// <summary>
    /// Class GetWorkItemFields.
    /// Implements the <see cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    /// </summary>
    /// <seealso cref="AzureDevOpsMgmt.Cmdlets.ApiCmdlet" />
    [Cmdlet(VerbsCommon.Get, "WorkItemFields")]
    public class GetWorkItemFields : ApiCmdlet
    {
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        [Parameter]
        public string FieldName { get; set; }

        /// <summary>
        /// Processes the cmdlet record.
        /// </summary>
        protected override void ProcessCmdletRecord()
        {
            if (this.FieldName != null)
            {
                var request = new RestRequest("wit/fields/{fieldName}");
                request.AddUrlSegment("fieldName", this.FieldName);
                var response = this.Client.Get<WorkItemField>(request);
                this.WriteObject(response, DevOpsModelTarget.WorkItem, ErrorCategory.NotSpecified, this);
            }
            else
            {
                var request = new RestRequest("wit/fields", Method.GET);
                var response = this.Client.Execute<ResponseModel<WorkItemField>>(request);
                this.WriteObject(response, DevOpsModelTarget.WorkItem, ErrorCategory.NotSpecified, this);
            }
        }
    }
}