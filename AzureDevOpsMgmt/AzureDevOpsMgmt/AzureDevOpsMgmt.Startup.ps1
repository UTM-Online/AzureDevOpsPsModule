$cmdlet = Get-Command -Name Import-AzureDevOpsConfiguration -Module AzureDevOpsMgmt
$cmdlet.Visibility = "Private"
AzureDevOpsMgmt\Import-Configuration