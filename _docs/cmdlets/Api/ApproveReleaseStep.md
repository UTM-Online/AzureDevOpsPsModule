---
title: Approve-AzureDevOpsReleaseStep
permalink: /docs/cmdlets/api/approvereleasestep/
redirect_from:
    - /docs/cmdlets/api/approverelease/
---

## Syntax
```powershell
Approve-AzureDevOpsReleaseStep
  -ApprovalId <int>
  -Reason <string>
```

<br>
## Description
Marks a release step as approved for continued processing.

<br>
## Example
```powershell
Approve-AzureDevOpsReleaseStep -ApprovalId 123456789 -Reason "Continued Release has been approved"
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
The reason message that will be included with the approval that is provided.

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False