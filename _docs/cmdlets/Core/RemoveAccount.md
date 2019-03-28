---
title: Remove-AzureDevOpsAccount
layout: docs
permalink: /docs/cmdlets/core/removeaccount/
redirect_from:
    - /docs/cmdlets/removeaccount/
---

## Syntax
```powershell
Remove-AzureDevOpsAccount 
  [-FriendlyName <string>]
```
<br>
## Description
Removes the specified account from the configuration repository

<br>
## Example
```powershell
Remove-AzureDevOpsAccount -FriendlyName MainAccount
```

<br>
## Parameters
### -FriendlyName

This is the friendly name you set on the account when you created it.

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False