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

	Process
	{
		$AzureDevOpsConfiguration.SetCurrentConnection($AccountName)
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