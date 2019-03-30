---
title: Get-AzureDevOpsBuild
permalink: /docs/cmdlets/api/getbuild/
---

## Syntax
### Get All builds for a specific build definition
```powershell
Get-AzureDevOpsBuild 
  [-BuildDefinitionId <int[]>]
```
<br>
### Get a spefic build
```powershell
Get-AzureDevOpsBuild
  [-BuildId <int>]
```

<br>
## Description
Gets a single build by specifying BuildId or get all builds for one or multiple build definitions by specifying the BuildDefinitionIds.

<br>
## Examples
```powershell
Get-AzureDevOpsBuild -BuildDefinitionId 123
```
<br>
```powershell
Get-AzureDevOpsBuild -BuildDefinitionId 123,456,789
```
<br>
```powershell
Get-AzureDevOpsBuild -BuildId 456
```

<br>
## Parameters
### -BuildDefinitionId
The Id number(s) for the build definition(s) you want to get all builds for.

<br>

Type: | int[]
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False

<br>

### -BuildId
The Id number of the specific build you want to get.

<br>

Type: | int
Position: | Named
Default Value: | None
Accept Pipeline Input: | False
Accept Wildcard Characters: | False