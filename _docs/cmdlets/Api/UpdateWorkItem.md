---
title: Update-AzureDevOpsWorkItem
layout: docs
permalink: /docs/cmdlets/api/updateworkitem/
---

## Syntax
```powershell
Update-AzureDevOpsWorkItem
  -Id <long>
  -UpdatedWorkItem <WorkItem>
  [-OriginalWorkItem <WorkItem>]
```

<br>
## Description
Takes a work item as input, compares it to the work item on the server, generates, and sends a patch file to update all changed properties of the work item provided.

<br>
## Example
```powershell
$wi = Get-AzureDevOpsWorkItem -Id 123456
$wi.fields."Microsoft.VSTS.Common.Priority" = 3
Update-AzureDevOpsWorkItem -Id $wi.Id -UpdateWorkItem $wi
```
<br>
```powershell
$wi = Get-AzureDevOpsWorkItem -Id 123456
$origionalwi = Get-AzureDevOpsWorkItem -Id 123456
$wi.fields."Microsoft.VSTS.Common.Priority" = 3
Update-AzureDevOpsWorkItem -Id $wi.Id -UpdateWorkItem $wi -OriginalWorkItem $origionalwi
```

<br>
## Parameters
### -Id
The Id of the workitem in the project your currently working on.

<br>

Type: | long
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>
### -UpdatedWorkItem
The updated version of the work item that you want to save in Azure Dev Ops

Type: | WorkItem
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>
### -OriginalWorkItem
The work item in its current state in Azure Dev Ops.  **This must be unmodified from the version stored in Azure or the update may not go as planned**

Type: | WorkItem
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False