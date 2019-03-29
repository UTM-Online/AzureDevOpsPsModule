---
title: Get-AzureDevOpsRelease
permalink: /docs/cmdlets/api/getrelease/
---

## Syntax
```powershell
Get-AzureDevOpsRelease
  -DefinitionId <int>
  [-Id <int>]
```
<br>
```powershell
Get-AzureDevOpsRelease
  [-PipelineInput <ReleaseDefinition>]
```

<br>
## Description
Gets a release from the release pipeline using a combination of DefinitionId and Id (if specified).

<br>
## Example
```powershell
Get-AzureDevOpsRelease -DefinitionId 123456
```
<br>
```powershell
Get-AzureDevOpsRelease -DefinitionId 123456 -Id 123
```

<br>
## Parameters
### -DefinitionId
The Id for the Release definition you want to fetch releases from.

<br>

Type: | int
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>
### -Id
The Id of the release you want to fetch.

<br>

Type: | int
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>
### -PipelineInput
Reserved for future use.

<br>

Type: | ReleaseDefinition
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False