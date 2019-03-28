---
title: Get-AzureDevOpsAccount
layout: docs
permalink: /docs/cmdlets/getaccount/
---

## Syntax
```powershell
Get-AzureDevOpsAccount
  [-FriendlyName <string>]
```

<br>
## Description
Gets all the accounts registered in the modules configuration repository.

If the `-FriendlyName` switch isn't provided then it gets all the accounts.  If a value is provided then it returns only the account specified.

<br>
## Examples
```powershell
Get-AzureDevOpsAccount
```
<br>
```powershell
Get-AzureDevOpsAccount -FriendlyName MainAccount
```

<br>
## Parameters
### -FriendlyName
The friendly name for the account that you specified when you added it using the `Add-AzureDevOpsAccount` cmdlet.

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False