---
title: Deny-AzureDevOpsReleaseStep
permalink: /docs/cmdlets/api/denyreleasetep/
---

## Syntax
```powershell
Deny-AzureDevOpsReleaseStep 
  -ApprovalId <int>
  -Reason <string>
```

<br>
### Description
Marks a release step as being denied for continued processing.

<br>
### Example
```powershell
Deny-AzureDevOpsReleaseStep -ApprovalId 123456789 -Reason "Continued release has been denied due to user acceptance tests failing"
```

<br>
## Parameters
### -ApprovalId
The ID for the current step in the release pipleine.

Type: | int
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -Reason
The reason message that will be included with the denial that is provided.

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False