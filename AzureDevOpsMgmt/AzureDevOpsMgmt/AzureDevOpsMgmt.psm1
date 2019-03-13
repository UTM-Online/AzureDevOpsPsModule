function CheckIsLoggedIn
{
    if($AzureDevOpsConnection.Connected)
    {
        return true
    }

    return false
}

function Connect-AzureDevOps
{

}