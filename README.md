# The Azure DevOps PowerShell Module
A PowerShell Module for interacting with Azure Dev Ops REST API.

| Master Branch Build / Release Status |
| :--: |
| [![Build status](https://dev.azure.com/utmo-public/PowerShellModules/_apis/build/status/AzureDevOps%20PS%20Module%20Build%20%26%20Publish-import)](https://dev.azure.com/utmo-public/PowerShellModules/_build/latest?definitionId=2) |

This project was born out of my own laziness when it came to calculating and updating my tasks in Azure Dev Ops.  Which is why it's started out with a very lite set of cmdlets.  But I plan on expanding the available cmdlets to mange work items but builds, releases, and various other aspects of the day to day life of a person who works primarily out of the Azure Dev Ops environment.

To get people started and make some value out right the module is designed to be scripted around to meet your own needs.  Have a specific workflow involving updating tasks you want to script or automate? Use the `Get-AzureDevOpsWorkItem` cmdlet to pull down the work item in its native form, make your updates and use the `Update-AzureDevOpsWorkItem` cmdlet to push your changes to your project.

Have a suggestion on improvements or changes to make this module more useful? Open an issue and mark it as an enhancement and I'll evaluate incorporating your request.  Feel like making the improvements your self?  Fork the repository and when you're finished open a PR to the dev/main branch.

[See the Module on PowerShell Gallery](https://www.powershellgallery.com/packages/AzureDevOpsMgmt)

[Documentation Avalible on Pages](http://devopsmodule.utmonline.net)
