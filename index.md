---
title: Home
layout: default
---

This project was born out of my own laziness when it came to calculating and updating my tasks in Azure Dev Ops.  Which is why it's started out with a very lite set of cmdlets.  But I plan on expanding the available cmdlets to mange work items but builds, releases, and various other aspects of the day to day life of a person who works primarily out of the Azure Dev Ops environment.

To get people started and make some value out right the module is designed to be scripted around to meet your own needs.  Have a specific workflow involving updating tasks you want to script or automate? Use the `Get-AzureDevOpsWorkItem` cmdlet to pull down the work item in its native form, make your updates and use the `Update-AzureDevOpsWorkItem` cmdlet to push your changes to your project.

Want to install the module? Just run:
```powershell
Install-Module -Name AzureDevOpsMgmt -Repository PSGallery -Scope:CurrentUser
```

Build / Release Status: [![Build status](https://dev.azure.com/utmo-public/PowerShellModules/_apis/build/status/AzureDevOps%20PS%20Module%20Build%20%26%20Publish)](https://dev.azure.com/utmo-public/PowerShellModules/_build/latest?definitionId=2)

[See the Module on PowerShell Gallery](https://www.powershellgallery.com/packages/AzureDevOpsMgmt)