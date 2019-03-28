---
title: Add-AzureDevOpsAccount
layout: docs
permalink: /docs/cmdlets/addaccount/
---

## Syntax
```powershell
Add-AzureDevOpsAccount
  [-AccountName <string>]
  [-FriendlyName <string>]
```
<br>
## Description
Adds an Azure Dev Ops Account to the configuration repository

<br>
## Example
```powershell
Add-AzureDevOpsAccount -AccountName ContosoDev -FriendlyName MainAccount
```
<br>
## Parameters
### -AccountName

This is the name of your account when you created it in Azure Dev Ops or Visual Studio Team Services.  It can be found by looking at the URL you go to to access your projects and repositories `<AccountName>.visualstudio.com` or `dev.azure.com/<AccountName>`.

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>
### -FriendlyName

This is a name that will be easy for you to remember, you can use the name of the account if that's what works for you.

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False