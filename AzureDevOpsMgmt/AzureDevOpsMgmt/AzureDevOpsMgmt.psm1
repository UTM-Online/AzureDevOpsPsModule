function CheckIsLoggedIn
{
	if(!$AzureDevOpsConfiguration.ReadyForCommands)
	{
		Write-Error -Message "Please run the `"Set-AzureDevOpsAccount`" cmdlet to set the current account context" -Category OperationStopped -ErrorAction Stop
	}
}

function Set-Account
{
	[CmdletBinding()]
	param
	(
		[string]$ProjectName
	)

	DynamicParam
	{
		$ParameterName = "AccountName"
		$RuntimeParameterDictionary = New-Object System.Management.Automation.RuntimeDefinedParameterDictionary
		$AttributeCollection = New-Object System.Collections.ObjectModel.Collection[System.Attribute]
		$ParameterAttribute = New-Object System.Management.Automation.ParameterAttribute
		$ParameterAttribute.Mandatory = $true
		$ParameterAttribute.Position = 1
		$AttributeCollection.Add($ParameterAttribute)
		$arraySet = $AzureDevOpsConfiguration.Accounts.GetAccountNames()
		$ValidateSetAttribute = New-Object System.Management.Automation.ValidateSetAttribute($arraySet)
		$AttributeCollection.Add($ValidateSetAttribute)
		$RuntimeParameter = New-Object System.Management.Automation.RuntimeDefinedParameter($ParameterName, [String[]], $AttributeCollection)
		$RuntimeParameterDictionary.Add($ParameterName, $RuntimeParameter)
		return $RuntimeParameterDictionary
	}

	Begin
	{
		$ErrorActionPreference = "Stop"
		if(!$AzureDevOpsConfiguration.Accounts.Accounts.Where({$_.FriendlyName -eq $PSBoundParameters["AccountName"]})[0].AccountProjects -contains $ProjectName)
		{
			Write-Debug -Message "Account Name is: $PSBoundParameters["AccountName"]"
			Write-Debug -Message "Found: $($AzureDevOpsConfiguration.Accounts.Accounts.Where({$_.FriendlyName -eq $PSBoundParameters["AccountName"]})[0])"

			Write-Error -Message "Specified Project is not registered for the selected account.  Please register the project with the account using the Add-AzureDevOpsAccountProject cmdlet" -ErrorAction Stop
		}
	}
	Process
	{
		$AzureDevOpsConfiguration.SetCurrentConnection($AccountName, $ProjectName)
		"Current Connection updated to: $AccountName"
	}
}

function Add-PatToken
{
	param
	(
		[string]$FriendlyName,
		[string]$UserName,
		[string]$PatToken
	)

	$AzureDevOpsConfiguration.Accounts.AddPatToken($FriendlyName, $UserName, $PatToken)
}

function Add-Account
{
	param
	(
		[string]$FriendlyName,
		[string]$AccountName,
		[string]$PatTokenFriendlyName
	)

	if($PatTokenFriendlyName)
	{
		[Guid]$PatTokenId = $AzureDevOpsConfiguration.Accounts.PatTokens | ?{$_.FriendlyName -eq $PatTokenFriendlyName} | Select -ExpandProperty Id
	}

	$AzureDevOpsConfiguration.Accounts.AddAccount($FriendlyName, $AccountName)
}

function Join-AccountAndPatToken
{
	param
	(
		[string]$AccountFriendlyName,
		[string]$PatTokenFriendlyName
	)

	$AzureDevOpsConfiguration.Accounts.LinkPatTokenToAccount($AccountFriendlyName, $PatTokenFriendlyName)
}

function Remove-Account
{
	param
	(
		[string]$FriendlyName
	)

	$AzureDevOpsConfiguration.Accounts.RemoveAccount($FriendlyName)
}

function Remove-PatToken
{
	param
	(
		[string]$FriendlyName
	)

	$AzureDevOpsConfiguration.Accounts.RemovePatToken($FriendlyName)
}

function Get-Account
{
	param
	(
		[string]$FriendlyName
	)

	if($FriendlyName)
	{
		return $AzureDevOpsConfiguration.Accounts.Accounts | ?{$_.FriendlyName -eq $FriendlyName}
	}

	$AzureDevOpsConfiguration.Accounts.Accounts
}

function Get-PatToken
{
	param
	(
		[string]$FriendlyName
	)

	if($FriendlyName)
	{
		return $AzureDevOpsConfiguration.Accounts.PatTokens | ?{$_.FriendlyName -eq $FriendlyName}
	}

	$AzureDevOpsConfiguration.Accounts.PatTokens
}

function Add-AccountProject
{
	param
	(
		[string]$AccountFriendlyName,
		[string]$ProjectName
	)

	$AzureDevOpsConfiguration.Accounts.Accounts.Where({$_.FriendlyName -eq $AccountFriendlyName})[0].AccountProjects.Add($ProjectName)
}