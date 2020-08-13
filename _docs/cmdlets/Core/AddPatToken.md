---
title: Add-AzureDevOpsPatToken
layout: docs
permalink: /docs/cmdlets/core/addpattoken/
redirect_from:
    - /docs/cmdlets/addpattoken/
---

## Syntax
```powershell
Add-AzureDevOpsPatToken
  [-FriendlyName <string>]
  [-UserName <string>]
  [-PatToken <string>]
```

<br>
## Description
Adds your PAT token to the windows credential manager and stores the tokens GUID, generated by the module, and its friendly name in the configuration repository.

The actual token is stored by Windows and the module uses a GUID it generates as it's way of retrieving that credential value from the operating system.

Additionally this value, and other pat tokens are not loaded in to memory by the module until you make the first call to the Azure Dev Ops Rest API.

## Example
```powershell
Add-AzureDevOpsPatToken -FriendlyName MainToken -UserName ExampleUser@contoso.com -PatToken "abcdefghijklmnopqrstuvwxyz0123456789"
```

## Parameters
### -FriendlyName

This is the easy to remember name you assign to this token.

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>
### -UserName

This is the user name that you use to login to azure dev ops, usualy in the form of a UPN. Example: `ExampleUser@contoso.com`.

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>
### -PatToken

This is the pat token given to you by Azure Dev Ops at your request in the security section of your profile.

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False