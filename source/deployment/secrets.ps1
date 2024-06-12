param ($keyVaultName)

# Will expose method NewPassword
. $PSScriptRoot\secretGenerator.ps1

################################################################
### Set Key Vault secrets to provided values.
################################################################
$sqlSaPasswordSecretName = "SQL-SA-PASSWORD"
$sqlAdminPasswordSecretName = "SQL-ADMIN-PASSWORD"

# We have to check whether all the relevant secrets are in there.
# If not, generate those secrets and store in Key Vault.
$query = "contains([].id, 'https://$($keyVaultName).vault.azure.net/secrets/$($sqlSaPasswordSecretName)')"
$exists = az keyvault secret list --vault-name $keyVaultName --query $query
if ($exists -eq "false") {
	az keyvault secret set --vault-name $keyVaultName --name $sqlSaPasswordSecretName --value $(NewPassword) --output none
	Write-Host "##[section]Set secret $sqlSaPasswordSecretName"
}

$query = "contains([].id, 'https://$($keyVaultName).vault.azure.net/secrets/$($sqlAdminPasswordSecretName)')"
$exists = az keyvault secret list --vault-name $keyVaultName --query $query
if ($exists -eq "false") {
	az keyvault secret set --vault-name $keyVaultName --name $sqlAdminPasswordSecretName --value $(NewPassword) --output none
	Write-Host "##[section]Set secret $sqlAdminPasswordSecretName"
}
