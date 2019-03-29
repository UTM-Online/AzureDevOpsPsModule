---
title: Update-AzureDevOpsRemainingWork
layout: docs
permalink: /docs/cmdlets/assistants/updateremainingwork/
---

## Syntax
```powershell
Update-AzureDevOpsRemainingWork
  [-Id] <long>
  [-WorkCompletedThisSession] <string[]>
  [[-Description] <string>]
```

<br>
## Description
Takes a time span string as input (the amount of time spent on a task), performs some calculations, and updates the work item with new values for completed and remaining work.

<br>
## Example
```powershell
Update-AzureDevOpsRemainingWork -Id 123456 -WorkCompletedThisSession "00:30:00"
```
<br>
```powershell
Update-AzureDevOpsRemainingWork -Id 123456 -WorkCompletedThisSession "00:30:00","01:15:00","04:03:00"
```
<br>
```powershell
Update-AzureDevOpsRemainingWork -Id 123456 -WorkCompletedThisSession "00:30:00" -Description "Fixing bug# 132456"
```

<br>
## Parameters
### -Id
The Id number of the work item you want to update your remaining work value on.

<br>

Type: | long
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -WorkCompletedThisSession
This parameter accepts an array of strings that are formatted in the string representation of a time span.  Where the first two numbers represent hours, followed by a colon, the second two numbers represent minutes, followed by a colon, and the last two numbers represent seconds.  Making this example &nbsp;`01:25:30`&nbsp; represent one hour twenty five minutes and thirty seconds.

<br>

Type: | string[]
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -Description
A description of the work you performed.  What you supply here will be added as a comment on the work item.

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False