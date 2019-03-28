---
title: Add-AzureDevOpsAccountProject
layout: docs
permalink: /docs/cmdlets/core/addaccountproject/
redirect_from:
    - /docs/cmdlets/addaccountproject/
---

## Syntax
```powershell
Add-AzureDevOpsAccountProject
  [-AccountFriendlyName <string>]
  [-ProjectName <string>]
```

<br>
## Description
Adds an individual project from the account to the account object in the configuration repository

<br>
## Example
```powershell
Add-AzureDevOpsAccountProject -AccountFriendlyName MainAccount -ProjectName "The Big Project"
```

<br>
## Parameters
### -AccountFriendlyName

This is the friendly name for the account that you created using `Add-AzureDevOpsAccount`.  This parameter supports tab compleation.

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>
### -ProjectName

This is the name of the project as it's displayed in the projects main page by navigating to `https://<Your Account>.visualstudio.com/<Your Project Name>`.  The project name will be displayed at the top of the project page.  Don't add your project name using the URL encoded value, i.e. if your project name has spaces then the url will be something like `Your%20Project%20Name`.  Currently this paramter does not perform a check for decoding the URL string so make sure you don't try and add a project name that looks like that.

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False