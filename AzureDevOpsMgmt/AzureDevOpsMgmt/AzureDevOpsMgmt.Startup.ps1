$cmdlet = Get-Command -Name Load-AzureDevOpsConfiguration -Module AzureDevOpsMgmt
$cmdlet.Visibility = "Private"
AzureDevOpsMgmt\Load-Configuration