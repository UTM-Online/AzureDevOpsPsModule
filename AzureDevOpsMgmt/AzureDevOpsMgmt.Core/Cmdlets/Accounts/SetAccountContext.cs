﻿// ***********************************************************************
// Assembly         : AzureDevOpsMgmt.Core
// Author           : Josh Irwin
// Created          : 08-15-2019
// ***********************************************************************
// <copyright file="SetAccountContext.cs" company="UTM Online">
//     Copyright ©  2019
// </copyright>
// ***********************************************************************

namespace AzureDevOpsMgmt.Cmdlets.Accounts
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Management.Automation;

    using AzureDevOpsMgmt.Exceptions;
    using AzureDevOpsMgmt.Helpers;
    using AzureDevOpsMgmt.Models;

    /// <summary>
    /// Class SetAccountContext.
    /// Implements the <see cref="System.Management.Automation.PSCmdlet" />
    /// Implements the <see cref="System.Management.Automation.IDynamicParameters" />
    /// </summary>
    /// <seealso cref="System.Management.Automation.PSCmdlet" />
    /// <seealso cref="System.Management.Automation.IDynamicParameters" />
    [Cmdlet(VerbsCommon.Set, "AccountContext")]
    public class SetAccountContext : PSCmdlet, IDynamicParameters
    {
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        [Parameter]
        public string ProjectName { get; set; }

        /// <summary>
        /// Returns an instance of an object that defines the
        /// dynamic parameters for this
        /// <see cref="T:System.Management.Automation.Cmdlet" /> or <see cref="T:System.Management.Automation.Provider.CmdletProvider" />.
        /// </summary>
        /// <returns>This method should return an object that has properties and fields
        /// decorated with parameter attributes similar to a
        /// <see cref="T:System.Management.Automation.Cmdlet" /> or <see cref="T:System.Management.Automation.Provider.CmdletProvider" />.
        /// These attributes include <see cref="T:System.Management.Automation.ParameterAttribute" />,
        /// <see cref="T:System.Management.Automation.AliasAttribute" />, argument transformation and
        /// validation attributes, etc.
        /// Alternately, it can return a
        /// <see cref="T:System.Management.Automation.RuntimeDefinedParameterDictionary" />
        /// instead.
        /// The <see cref="T:System.Management.Automation.Cmdlet" /> or <see cref="T:System.Management.Automation.Provider.CmdletProvider" />
        /// should hold on to a reference to the object which it returns from
        /// this method, since the argument values for the dynamic parameters
        /// specified by that object will be set in that object.
        /// This method will be called after all formal (command-line)
        /// parameters are set, but before <see cref="M:System.Management.Automation.Cmdlet.BeginProcessing" />
        /// is called and before any incoming pipeline objects are read.
        /// Therefore, parameters which allow input from the pipeline
        /// may not be set at the time this method is called,
        /// even if the parameters are mandatory.</returns>
        public object GetDynamicParameters()
        {
            var parameterName = "AccountName";
            var runTimeParameterDictionary = new RuntimeDefinedParameterDictionary();
            var attributeCollection = new Collection<Attribute>();
            var parameterAttribute = new ParameterAttribute();
            parameterAttribute.Mandatory = true;
            parameterAttribute.Position = 1;
            attributeCollection.Add(parameterAttribute);
            var arraySet = AzureDevOpsConfiguration.Config.Accounts.GetAccountNames();
            var validateSetAttribute = new ValidateSetAttribute(arraySet);
            attributeCollection.Add(validateSetAttribute);
            var runtimeParameter = new RuntimeDefinedParameter(parameterName, typeof(string), attributeCollection);
            runTimeParameterDictionary.Add(parameterName, runtimeParameter);
            return runTimeParameterDictionary;
        }

        /// <summary>
        /// When overridden in the derived class, performs initialization
        /// of command execution.
        /// Default implementation in the base class just returns.
        /// </summary>
        /// <exception cref="T:System.Management.Automation.PipelineStoppedException">The pipeline has already been terminated, or was terminated
        /// during the execution of this method.
        /// The Cmdlet should generally just allow PipelineStoppedException
        /// to percolate up to the caller of ProcessRecord etc.</exception>
        protected override void BeginProcessing()
        {
            AzureDevOpsAccount account =
                AzureDevOpsConfiguration.Config.Accounts.Accounts.FirstOrDefault(i => i.FriendlyName.Equals(this.GetPsBoundParameter<string>("AccountName"), StringComparison.OrdinalIgnoreCase));

            if ((account == null) | (account == default(AzureDevOpsAccount)))
            {
                this.WriteError(
                                new ErrorRecord(
                                                new InvalidOperationException("The specified account was not found"),
                                                "AzureDevOpsMgmt.Accounts.SetAccountContext.AccountNotFoundException",
                                                ErrorCategory.InvalidArgument,
                                                this.GetPsBoundParameter<string>("AccountName")));
            }
            else if (!account.AccountProjects.Contains(this.ProjectName, StringComparer.OrdinalIgnoreCase))
            {
                this.WriteError(
                                new ErrorRecord(
                                                new InvalidOperationException("The specified project was not found"),
                                                "AzureDevOpsMgmt.Accounts.SetAccountContext.ProjectNotFoundException",
                                                ErrorCategory.InvalidArgument,
                                                this.ProjectName));
            }
#pragma warning disable 618,612
            else if (account.TokenId == null)
#pragma warning restore 618,612
            {
                this.WriteError(
                                new NoPatTokenLinkedException(account.FriendlyName),
                                this.BuildStandardErrorId(DevOpsModelTarget.InternalOperation, "PatTokenNotLinked"),
                                ErrorCategory.InvalidOperation,
                                this);
            }
        }

        /// <summary>
        /// When overridden in the derived class, performs clean-up
        /// after the command execution.
        /// Default implementation in the base class just returns.
        /// </summary>
        /// <exception cref="T:System.Management.Automation.PipelineStoppedException">The pipeline has already been terminated, or was terminated
        /// during the execution of this method.
        /// The Cmdlet should generally just allow PipelineStoppedException
        /// to percolate up to the caller of ProcessRecord etc.</exception>
        protected override void EndProcessing()
        {
            this.WriteObject("Connection context has been set");
        }

        /// <summary>
        /// When overridden in the derived class, performs execution
        /// of the command.
        /// </summary>
        protected override void ProcessRecord()
        {
            var newCurrentAccount = AzureDevOpsConfiguration.Config.Accounts.Accounts.First(i => i.FriendlyName.Equals(this.GetPsBoundParameter<string>("AccountName"), StringComparison.OrdinalIgnoreCase));

            #pragma warning disable 618,612
            var newPatToken = AzureDevOpsConfiguration.Config.Accounts.PatTokens.First(i => i.Id == newCurrentAccount.TokenId);
            #pragma warning restore 618,612

            if (AzureDevOpsConfiguration.Config.CurrentConnection == null)
            {
                AzureDevOpsConfiguration.Config.CurrentConnection = new CurrentConnection(newCurrentAccount, newPatToken, this.ProjectName);
            }
            else
            {
                var connection = AzureDevOpsConfiguration.Config.CurrentConnection;

                connection.Account = newCurrentAccount;
                connection.ProjectName = this.ProjectName;
                connection.Token = newPatToken;

                AzureDevOpsConfiguration.Config.CurrentConnection = connection;
            }
        }
    }
}
