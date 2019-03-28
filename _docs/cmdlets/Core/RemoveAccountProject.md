---
title: Remove-AzureDevOpsAccountProject
layout: docs
permalink: /docs/cmdlets/core/removeaccountproject/
redirect_from:
    - /docs/cmdlets/removeaccountproject/
---

## Syntax
```powershell
Remove-AzureDevOpsAccountProject
  [-AccountFriendlyName <string>]
  [-ProjectName <string>]
```

<br>
## Description
Remove the Project from the specified Account in the configuration repository.

<br>
## Example
```powershell
Remove-AzureDevOpsAccountProject -AccountFriendlyName MainAccount -ProjectName "The Big Project"
```

<br>
## Parameters
### -AccountFriendlyName
This is the friendly name of the account where the project resides that you want to remove.

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>
### -ProjectName
The name of the project you added to the account that you want to remove.

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False