---
title: Set-AzureDevOpsAccountContext
layout: docs
permalink: /docs/cmdlets/core/setaccountcontext/
---

## Syntax
```powershell
Set-AzureDevOpsAccountContext
  [-AccountName <AzureDevOpsAccount>]
  [-ProjectName <string>]
```

<br>
## Description
Sets the Account and project that future cmdlets will be executing against. 

**<u><i>NOTE:</i> This must be run prior to the execution of any cmdlets that interact with the Azure Dev Ops Rest API</u>**

<br>
## Example
```powershell
Set-AzureDevOpsAccountContext -AccountName MainAccount -ProjectName "The Big Project"
```

<br>
## Parameters
### -AccountName
The name of the account that you will be working with.

<br>

Type: | AzureDevOpsAccount
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>
### -ProjectName
The name of the project you want to work with

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False