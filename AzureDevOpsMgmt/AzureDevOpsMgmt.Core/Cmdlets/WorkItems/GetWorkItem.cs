// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : joirwi
// Created          : 03-19-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-19-2019
// ***********************************************************************
// <copyright file="GetWorkItem.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AzureDevOpsMgmt.Cmdlets.WorkItems
{
    using System.Diagnostics.CodeAnalysis;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    using RestSharp;

    /// <summary>
    /// Class GetWorkItem.
    /// Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    [Cmdlet(VerbsCommon.Get, "WorkItem")]
    public class GetWorkItem : ApiCmdlet
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Parameter]
        public long Id { get; set; }

        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        /// <exception cref="T:System.Management.Automation.PipelineStoppedException">The pipeline has already been terminated, or was terminated
        ///             during the execution of this method.
        ///             The Cmdlet should generally just allow PipelineStoppedException
        ///             to percolate up to the caller of ProcessRecord etc.
        /// </exception>
        /// <exception cref="T:System.Management.Automation.SessionStateUnauthorizedAccessException">If the variable is read-only or constant.</exception>
        /// <exception cref="T:System.Management.Automation.ProviderInvocationException">If the provider threw an exception.</exception>
        /// <exception cref="T:System.Management.Automation.DriveNotFoundException">If the <paramref name="name" /> refers to a drive that could not be found.</exception>
        /// <exception cref="T:System.Management.Automation.ProviderNotFoundException">If the <paramref name="name" /> refers to a provider that could not be found.</exception>
        /// <exception cref="T:System.Management.Automation.SessionStateOverflowException">If the maximum number of variables has been reached for this scope.</exception>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        protected override void ProcessCmdletRecord()
        {
            var workItemRequest = new RestRequest($"wit/workitems/{this.Id}", Method.GET);
            var workItemResponse = this.client.Execute<WorkItem>(workItemRequest);

            this.WriteObject(workItemResponse, DevOpsModelTarget.WorkItem, ErrorCategory.NotSpecified, this);
        }
    }
}
