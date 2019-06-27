// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : joirwi
// Created          : 03-21-2019
//
// Last Modified By : joirwi
// Last Modified On : 03-21-2019
// ***********************************************************************
// <copyright file="WorkItemHelpers.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AzureDevOpsMgmt.Helpers
{
    using System.Collections.Generic;

    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.WebApi;

    /// <summary>
    ///     Class WorkItemHelpers.
    /// </summary>
    public static class WorkItemHelpers
    {
        /// <summary>
        ///     Deeps the copy.
        /// </summary>
        /// <param name="wi">The wi.</param>
        /// <returns>A clone of the original WorkItem.</returns>
        public static WorkItem DeepCopy(this WorkItem wi)
        {
            var newWorkItem = new WorkItem
                                  {
                                      Id = wi.Id,
                                      Rev = wi.Rev,
                                      Fields = wi.Fields == null ? null : new Dictionary<string, object>(),
                                      Links = wi.Links == null ? null : new ReferenceLinks(),
                                      Relations = wi.Relations == null ? null : new List<WorkItemRelation>(),
                                      Url = wi.Url
                                  };

            if (newWorkItem.Fields != null && wi.Fields?.Count > 0)
            {
                foreach (var item in wi.Fields)
                {
                    newWorkItem.Fields.Add(item.Key, item.Value);
                }
            }

            if (newWorkItem.Links != null && wi.Links?.Links != null && wi.Links.Links.Count > 0)
            {
                foreach (var item in wi.Links.Links)
                {
                    newWorkItem.Links.AddLink(item.Key, ((ReferenceLink)item.Value).Href);
                }
            }

            if (newWorkItem.Relations != null && wi.Relations?.Count > 0)
            {
                foreach (var item in wi.Relations)
                {
                    newWorkItem.Relations.Add(item.DeepCopy());
                }
            }

            return newWorkItem;
        }

        /// <summary>  Performs a deep copy of the source object</summary>
        /// <param name="wir">The Work Item Relationship</param>
        /// <returns>A WorkItemRelation.</returns>
        private static WorkItemRelation DeepCopy(this WorkItemRelation wir)
        {
            var newWorkItemRelation = new WorkItemRelation
                                          {
                                              Attributes = new Dictionary<string, object>(),
                                              Rel = wir.Rel,
                                              Title = wir.Title,
                                              Url = wir.Url
                                          };

            if (wir.Attributes != null && wir.Attributes.Count > 0)
            {
                foreach (var item in wir.Attributes)
                {
                    newWorkItemRelation.Attributes.Add(item.Key, item.Value);
                }
            }

            return newWorkItemRelation;
        }
    }
}