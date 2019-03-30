---
title: Set-AzureDevOpsStartUpAccount
permalink: /docs/cmdlets/core/setstartupaccount/
redirect_for:
    - /docs/cmdlets/core/setdefaultaccount/
---

## Syntax
```powershell
Set-AzureDevOpsStartUpAccount
  -AccountFriendlyName <string>
  -ProjectName <string>
```

<br>
## Description
This cmdlet allows you set configure a default account that you want to have set as the current context when the module loads.

Basically it runs `Set-AzureDevOpsAccountContext` on your behalf when the module loads with the account and project that you specify.

<br>
## Example
```powershell
Set-AzureDevOpsStartUpAccount -AccountFriendlyName MainAccount -ProjectName "The Big Project"
```

<br>
## Parameters
### -AccountFriendlyName
The friendly name of the account that you specified when it was added to the configuration repository.

<br>
### -ProjectName
The name project name you want to have loaded automatically when the module is loaded.