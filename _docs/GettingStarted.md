---
title: Getting Started
layout: default
redirect_from:
    - /docs
---

# How to get started using the Azure DevOps Management PowerShell Module

The module is designed to be both easy to use but also highly flexible well also providing safe guards from doing potentially destructive things.

Quick Start Steps:

1) Run `Add-AzureDevOpsAccount -AccountName <Account Name> -FriendlyName <A name you use to identify the account>`
1) Run `Add-AzureDevOpsAccountProject -AccountFriendlyName <Name> -ProjectName <Project Name>`
1) Run `Add-AzureDevOpsPatToken -FriendlyName <Friendly name for the token> -UserName <Should be in the form of a UPN / Email Address> -PatToken <Value for your pat token obtained in the Azure UI>`
1) Run `Join-AzureDevOpsAccountAndPatToken -AccountFriendlyName <Account Friendly Name> -PatTokenFriendlyName <Pat Token Friendly Name>`

Now that you've configured the account you want to use in the module all that's left is to tell it that you want to use the account you've created.  To do that run the following command:

* `Set-AzureDevOpsAccountContext -AccountName <Account Name> -ProjectName <Project Name>`

You're now set to start using the Rest API based commands!