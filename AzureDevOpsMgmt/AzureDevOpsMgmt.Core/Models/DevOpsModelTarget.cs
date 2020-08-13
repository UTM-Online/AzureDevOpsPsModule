// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : joirwi
// Created          : 03-26-2019
// ***********************************************************************
// <copyright file="DevOpsModelTarget.cs" company="Microsoft">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************
namespace AzureDevOpsMgmt.Models
{
    /// <summary>
    /// An enumeration of possible resource types the module can target in Azure Dev Ops
    /// This is primary used for error reporting purposes in the generation of the Standard Error ID.
    /// </summary>
    public enum DevOpsModelTarget
    {
        /// <summary>
        /// Work Item Resources
        /// </summary>
        WorkItem,

        /// <summary>
        /// Build Resources
        /// </summary>
        Build,

        /// <summary>
        /// Release Resources
        /// </summary>
        Release,

        /// <summary>
        /// Indicates that the error took place during internal processing of an operation.
        /// </summary>
        InternalOperation,

        /// <summary>  Area's and Iterations Resources</summary>
        AreasAndIterations,


        /// <summary>  A User Defined ADO Target</summary>
        CustomTarget
    }
}
