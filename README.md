# The Azure DevOps PowerShell Module
A PowerShell Module for interacting with Azure Dev Ops REST API.

[![Board Status](https://dev.azure.com/utmo-public/49d97751-6a82-4abe-95eb-10c5d5d655ec/a98a395b-009a-4e58-8e6b-8b8cf403bd0f/_apis/work/boardbadge/18a77344-3edd-4bca-9a87-6b8c57a60e5e?columnOptions=1)](https://dev.azure.com/utmo-public/49d97751-6a82-4abe-95eb-10c5d5d655ec/_boards/board/t/a98a395b-009a-4e58-8e6b-8b8cf403bd0f/Microsoft.RequirementCategory)

| Master Branch Build / Release Status |
| :--: |
| [![Build status](https://dev.azure.com/utmo-public/PowerShellModules/_apis/build/status/AzureDevOps%20PS%20Module%20Build%20%26%20Publish-import)](https://dev.azure.com/utmo-public/PowerShellModules/_build/latest?definitionId=2) / [![Release Status](https://vsrm.dev.azure.com/utmo-public/_apis/public/Release/badge/49d97751-6a82-4abe-95eb-10c5d5d655ec/1/1)](https://dev.azure.com/utmo-public/PowerShellModules/_release?definitionId=1&_a=releases&view=all) |

[See the Module on PowerShell Gallery](https://www.powershellgallery.com/packages/AzureDevOpsMgmt) | [Documentation Avalible on Pages](http://devopsmodule.utmonline.net)

[Project Work & Dashboard](https://dev.azure.com/utmo-public/PowerShellModules/_dashboards/dashboard/6379813e-ebd2-4c81-a3c1-63426adc1f07) | [Submit Cmdlet or Feature Request](https://forms.office.com/Pages/ResponsePage.aspx?id=Fzdqg6G0lECAxpK5EeA3lXem5Ywn_UpKoT8opq6F0kpUOTJWM0ZES1hJNllaRTZTM01BTFdRRVdQUy4u)

This project was born out of my own laziness when it came to calculating and updating my tasks in Azure Dev Ops.  Which is why it's started out with a very lite set of cmdlets.  But I plan on expanding the available cmdlets to mange work items but builds, releases, and various other aspects of the day to day life of a person who works primarily out of the Azure Dev Ops environment.

To get people started and make some value out right the module is designed to be scripted around to meet your own needs.  Have a specific workflow involving updating tasks you want to script or automate? Use the `Get-AzureDevOpsWorkItem` cmdlet to pull down the work item in its native form, make your updates and use the `Update-AzureDevOpsWorkItem` cmdlet to push your changes to your project.

Have a suggestion on improvements or changes to make this module more useful? Open an issue and mark it as an enhancement and I'll evaluate incorporating your request.  Feel like making the improvements your self?  Fork the repository and when you're finished open a PR to the dev/main branch.
