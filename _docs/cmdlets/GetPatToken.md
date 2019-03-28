---
title: Get-AzureDevOpsPatToken
layout: docs
permalink: /docs/cmdlets/getpattoken/
---

## Syntax
```powershell
Get-AzureDevOpsPatToken
  [-FriendlyName <string>]
```

<br>
## Description
Display a list of PAT Tokens currently stored in the configuration repository

If the `-FriendlyName` switch isn't provided then it gets all the tokens.  If a value is provided then it returns only the token specified.

<br>
## Example
```powershell
Get-AzureDevOpsPatToken
```

<br>

```powershell
Get-AzureDevOpsPatToken -FriendlyName MyMainToken
```

<br>

## Parameters
### -FriendlyName
The friendly name for the token that you specified when you added it using the `Add-AzureDevOpsPatToken` cmdlet.