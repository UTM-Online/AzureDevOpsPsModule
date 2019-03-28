---
title: Getting Started
layout: docs
permalink: /docs/
redirect_from:
    - /docs/home/
    - /docs/quickstart/
---

The module is designed to be both easy to use but also highly flexible well also providing safe guards from doing potentially destructive things.

Quick Start Steps:

1. Add your account to the modules configuration repository by running the following cmdlet:
`Add-AzureDevOpsAccount -AccountName <string> -FriendlyName <string>`
2. Add the project you want to work with to the account you just created by running the following cmdlet:
`Add-AzureDevOpsAccountProject -AccountFriendlyName <string> -ProjectName <string>`
3. Add your PAT Token, otherwise known as your Personal Acess Token, to the modules configuration repository by running the following cmdlet:
`Add-AzureDevOpsPatToken -FriendlyName <string> -UserName <string> -PatToken <string>`
4. Link your account to your PAT token so the module knows how to authenticate to the REST API by running the following cmdlet:
`Join-AzureDevOpsAccountAndPatToken -AccountFriendlyName <string> -PatTokenFriendlyName <string>`

Now that you've configured the account you want to use in the module all that's left is to tell it that you want to use the account you've created.  To do that run the following command:

`Set-AzureDevOpsAccountContext -AccountName <string> -ProjectName <string>`

You're now set to start using the Rest API based commands!