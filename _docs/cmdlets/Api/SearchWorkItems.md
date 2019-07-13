---
title: Search-AzureDevOpsWorkItems
permalink: /docs/cmdlets/api/searchworkitems/
redirect_from:
      - /docs/cmdlets/api/searchworkitems/
---

## Syntax
```powershell
Search-AzureDevOpsWorkItem
    -TeamName <string>
    -Query <string>
```

<br>
## Description
Using the provided query string, performs a search of work items in your project / team.

<br>
## Example
```powershell
Search-AzureDevopsWorkItems -TeamName "MyTeam" -Query "Select [System.Id], [System.Title], [System.State] From WorkItems Where [System.WorkItemType] = 'Task' AND [State] <> 'Closed' AND [State] <> 'Removed' order by [Microsoft.VSTS.Common.Priority] asc, [System.CreatedDate] desc"
```

<br>
## Parameters
### -TeamName
    The name of the team you want your query to run under.

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -Query
The query formated using the ["Work Item Query Language"](https://docs.microsoft.com/en-us/azure/devops/boards/queries/wiql-syntax?view=azure-devops) syntax

Type: | string
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False