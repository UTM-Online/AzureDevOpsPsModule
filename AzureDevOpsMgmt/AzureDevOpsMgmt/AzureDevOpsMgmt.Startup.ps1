$cmdlet = Get-Command -Name Import-AzureDevOpsConfiguration -Module AzureDevOpsMgmt
$cmdlet.Visibility = "Private"
AzureDevOpsMgmt\Import-Configuration

if($AzureDevOpsConfiguration.Config.CurrentConnection)
{
	Write-Host -Object "Default configurations have been loaded.`r`nAccount: $($AzureDevOpsConfiguration.Config.CurrentConnection.DefaultAccount)`r`nProject: $($AzureDevOpsConfiguration.Config.CurrentConnection.DefaultProject)"
}