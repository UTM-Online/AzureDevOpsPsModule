---
title: Get-AzureDevOpsWorkItemFields
layout: docs
permalink: /docs/cmdlets/api/getworkitemfields/
---

## Syntax
```powershell
Get-AzureDevOpsWorkItemFields
  [-FieldName <string>]
```

<br>
## Description
Retrieves a list of all fields in the project used in **All** work item types.

<br>
## Example
```powershell
Get-AzureDevOpsWorkItemFields
```

```powershell
Get-AzureDevOpsWorkItemFields -FieldName "Description"
```

<br>
## Parameters
### -FieldName
The name of the field as it appears in the UI.

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False