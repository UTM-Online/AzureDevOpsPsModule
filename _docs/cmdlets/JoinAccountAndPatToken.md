---
title: Join-AzureDevOpsAccountAndPatToken
layout: docs
permalink: /docs/cmdlets/joinaccountandpattoken/
---

## Syntax
```powershell
Join-AzureDevOpsAccountAndPatToken
  [-AccountFriendlyName <string>]
  [-PatTokenFriendlyName <string>]
```

<br>
## Description
Links an account in the configuration repository to a PAT token in the configuration repository.

<br>
## Example
```powershell
Join-AzureDevOpsAccountAndPatToken -AccountFriendlyName MainAccount -PatTokenFriendlyName MainToken
```

<br>
## Parameters
### -AccountFriendlyName
The friendly name of the account you specified when you added it to the configuration repository.

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>
### -PatTokenFriendlyName
The friendly name of the Pat token that you specified when you added it to the configuration repository.

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False