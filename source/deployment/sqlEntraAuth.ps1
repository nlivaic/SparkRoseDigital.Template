param ($sqlAdminUserName, $sqlAdminUserPassword, $sqlUsersGroupName, $resourceGroupName, $appServiceWebName, $sqlServerName)

$domain = (az rest --method get --url 'https://graph.microsoft.com/v1.0/domains?$select=id' --query value --output tsv)

####################################################
### Create and Populate Azure Sql Admin
####################################################
Write-Host "##[warning]--- Create and Populate Azure Sql Admin Group - START ---"
$sqlAdminUserPrincipalName = $sqlAdminUserName + '@' + $domain
$sqlAdminUserId = (az ad user list --filter "userPrincipalName eq '$sqlAdminUserPrincipalName'" --query '[].id' --output tsv)
If ($sqlAdminUserId -eq $null) {
    $sqlAdminUserId = (az ad user create --display-name $sqlAdminUserName --password $sqlAdminUserPassword --user-principal-name $sqlAdminUserPrincipalName --query id --output tsv)
    Write-Host "##[section]Created Entra user '$sqlAdminUserPrincipalName' with user Id: $sqlAdminUserId"
}
Write-Host "##[warning]--- Create and Populate Azure Sql Admin Group - END ---"

################################################################
### Assign Azure Sql Admin to relevant SQL Server roles
################################################################
Write-Host "##[warning]--- Assign Azure Sql Admin Group to roles - START ---"
$rgId = az group show --resource-group $resourceGroupName --query id --output tsv
# Scoped to resource group
# Reader
az role assignment create --assignee $sqlAdminUserId --role "acdd72a7-3385-48ef-bd42-f606fba81ae7" --scope $rgId
Write-Host "##[section]Added Entra user '$sqlAdminUserId' to role 'Reader'"
# SQL Server Contributor
az role assignment create --assignee $sqlAdminUserId --role "6d8ee4ec-f05a-4a1d-8b00-a9b17e38b437" --scope $rgId
Write-Host "##[section]Added Entra user '$sqlAdminUserId' to role 'SQL Server Contributor'"
# SQL Security Manager
az role assignment create --assignee $sqlAdminUserId --role "056cd41c-7e88-42e1-933e-88ba6a50c9c3" --scope $rgId
Write-Host "##[section]Added Entra user '$sqlAdminUserId' to role 'SQL Security Manager'"
Write-Host "##[warning]--- Assign Azure Sql Admin Group to roles - END ---"

####################################################
### Create Azure Sql User Group
####################################################
Write-Host "##[warning]--- Create and Populate Azure Sql User Group - START ---"
$sqlUsersGroupId=(az ad group list --filter "displayName eq '$sqlUsersGroupName'" --query '[].id' --output tsv)
If ($sqlUsersGroupId -eq $null) {
    $sqlUsersGroupId=(az ad group create --display-name $sqlUsersGroupName --mail-nickname $sqlUsersGroupName --query id --output tsv)
    Write-Host "##[section]Created Entra group '$sqlUsersGroupName' with group Id: $sqlUsersGroupId"
}
Write-Host "##[warning]--- Create and Populate Azure Sql User Group - END ---"

####################################################
### Set Sql Server Admin
####################################################
Write-Host "##[warning]--- Set Sql Server Admin - START ---"
$sqlServerAdmin = az sql server ad-admin list --resource-group $resourceGroupName --server-name $sqlServerName --query '[].login' --output tsv
If ($sqlServerAdmin -eq $null) {
    az sql server ad-admin create --display-name $sqlAdminUserPrincipalName --object-id $sqlAdminUserId --resource-group $resourceGroupName --server-name $sqlServerName
    Write-Host "##[section]Set user '$sqlAdminUserName' as Sql Server Admin for Sql server '$sqlServerName'"
}
Write-Host "##[warning]--- Set Sql Server Admin - END ---"

####################################################
### AAD only auth
####################################################
Write-Host "##[warning]--- AAD only auth - START ---"
$isAdOnlyAuthEnabled = (az sql server ad-only-auth get --resource-group $resourceGroupName --name $sqlServerName --query azureAdOnlyAuthentication --output tsv)
If ($isAdOnlyAuthEnabled -eq "false") {
    az sql server ad-only-auth enable --resource-group $resourceGroupName --name $sqlServerName
    Write-Host "##[section]Enabled Azure AD-only auth for Sql server '$sqlServerName'"
}
Write-Host "##[warning]--- AAD only auth - END ---"

####################################################
### Create Managed Identity for Web API
####################################################
Write-Host "##[warning]--- Create Managed Identity for Web API - START ---"
# Enable managed identity on app
$managedIdentityId = (az webapp identity show --resource-group $resourceGroupName --name $appServiceWebName --query principalId --output tsv)
If ($managedIdentityId -eq $null) {
    $managedIdentityId = (az webapp identity assign --resource-group $resourceGroupName --name $appServiceWebName  --query principalId --output tsv)
    Write-Host "##[section]Created system-assigned managed identity for '$appServiceWebName' with Id: $managedIdentityId"
}
# Add Managed Identity to sqlusersgroup
$isInGroup = (az ad group member check --group $sqlUsersGroupId --member-id $managedIdentityId  --query value --output tsv)
if ($isInGroup -eq 'false') {
    az ad group member add --group $sqlUsersGroupId --member-id $managedIdentityId
    Write-Host "##[section]Added Entra Managed Identity '$appServiceWebName' with id '$managedIdentityId' to group: $sqlUsersGroupId"
}
Write-Host "##[warning]--- Create Managed Identity for Web API - END ---"
Write-Host "##vso[task.setvariable variable=sqlAdminUserPrincipalName;isoutput=true]$sqlAdminUserPrincipalName"
