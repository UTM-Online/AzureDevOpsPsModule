---
title: New-AzureDevOpsWorkItem
permalink: /docs/cmdlets/api/newworkitem/
---

## Syntax
#### Including Estimation
```powershell
New-AzureDevOpsWorkItem
    [-Type <string>]
    [-Name <string>]
    [-AreaPath <string>]
    [-IterationPath <string>]
    [-BestCaseEstimate <int>]
    [-MostLikelyCaseEstimate <int>]
    [-WostCaseEstimate <int>]
    -ParentId <long>
    -Description <string>
    -Tags <string[]>
    -TriageApproved <switch>
    -OpenOnCompletion <switch>
    -SkipSelfAssignment <switch>
```

<br>

### Without Estimation
```powershell
New-AzureDevOpsWorkItem
    [-Type <string>]
    [-Name <string>]
    [-AreaPath <string>]
    [-IterationPath <string>]
    [-SkipTaskEstimation <switch>]
    -ParentId <long>
    -Description <string>
    -Tags <string[]>
    -TriageApproved <switch>
    -OpenOnCompletion <switch>
    -SkipSelfAssignment <switch>
```

<br>
## Description
Creates a new task using the current Account & Project as well as the specified input paramters.

<br>
## Examples
TODO

<br>
## Parameters
### -Type
Options are Task or Bug

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -Name
The title of the work item

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -AreaPath
The area path the work item should be created under

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -IterationPath
The iteration path the work item should be created under

<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -ParentId
The parent of the work item, if any

<br>

Type: | long
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -BestCaseEstimate
In the best case or happy path scenario how long would this work take?

<br>

Type: | int
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -MostLikelyCaseEstimate
Things being as usual how long would this work take?


<br>

Type: | int
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -WorstCaseEstimate
If everything that can go wrong has gone wrong how long would this work take?


<br>

Type: | int
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -Description
The value provided to the description field of the work item being created


<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -Tags
The tags to apply to the work item represented as an array of strings


<br>

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -TriageApproved
Set the TriageApproved Field to Approved


<br>

Type: | switch
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -OpenOnCompletion
Used to specify that the work item should be opened in the browser once created


<br>

Type: | switch
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -SkipSelfAssignment
By default new work items are assigned to the owner of the PAT token.  If you don't want this to happen include this switch when creating new work items and they will be assigned to no one when created.


<br>

Type: | switch
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -SkipTaskEstimation
Include this parameter when you don't want to estimate the task you are creating


<br>

Type: | switch
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

