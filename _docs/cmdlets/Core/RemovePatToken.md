---
title: Remove-AzureDevOpsPatToken
layout: docs
permalink: /docs/cmdlets/core/removepattoken/
---

## Syntax
```powershell
Remove-AzureDevOpsPatToken
  [-FriendlyName <string>]
```

<br>
# Description
Removes the Token meta-data from the configuration repository as well as the associated Token from the Windows Credential manager

<br>
## Example
```powershell
Remove-AzureDevOpsPatToken -FriendlyName "My Pat Token Friendly Name"
```

<br>
## Parameters
### -FriendlyName

The friendly name of the pat token you wish to perform the remove operation on.

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False