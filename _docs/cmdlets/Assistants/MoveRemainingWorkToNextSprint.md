---
title: Move-AzureDevOpsRemainingWorkToNextSprint
layout: docs
permalink: /docs/cmdlets/assistants/moveremainingworktonextsprint/
---

## Syntax
```powershell
Move-AzureDevOpsRemainingWorkToNextSprint
    -MoveOption <MoveOption>
    -SourceSprint <string>
    -DestinationSprint <string>
```

<br>
## Description
Get's all incompleate work assigned to the owner of the PAT token in the selected sprint and moves it to the next iteration

<br>
## Example
```powershell
Move-AzureDevOpsRemainingWorkToNextSprint
```

<br>
## Parameters
### -MoveOption
Options are: None | CopyParent | MoveParent

<br>

Type: | enum (MoveOption)
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -SourceSprint
The sprint from which to move work items from

<br>

Type: | string
Type: | enum (MoveOption)
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -DestinationSprint
The sprint to move the work items to

<br>

Type: | string
Type: | enum (MoveOption)
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False