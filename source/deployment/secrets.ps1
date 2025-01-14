param ($keyVaultName, $dbConnectionString, $messageBrokerConnectionString, $appInsightsConnectionString)

# Will expose method NewPassword
. $PSScriptRoot\secretGenerator.ps1

################################################################
### Set Key Vault secrets to provided values.
################################################################
$sqlSaPasswordSecretName = "SqlSaPassword"
$sqlAdminPasswordSecretName = "SqlAdminPassword"
$dbConnectionName = 'SparkRoseDigital_TemplateDbConnection'
$messageBrokerName = 'MessageBroker'
$applicationInsightsConnectionName = 'APPLICATIONINSIGHTS_CONNECTION_STRING'

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

################################################################
### Set connection strings.
################################################################
if ($dbConnectionString -ne $null) {
	$query = "contains([].id, 'https://$($keyVaultName).vault.azure.net/secrets/$($dbConnectionName)')"
	$exists = az keyvault secret list --vault-name $keyVaultName --query $query
	if ($exists -eq "false") {
		az keyvault secret set --vault-name $keyVaultName --name $dbConnectionName --value "$($dbConnectionString)" --output none
		Write-Host "##[section]Set secret $dbConnectionName"
	}
}

if ($messageBrokerConnectionString -ne $null) {
	$query = "contains([].id, 'https://$($keyVaultName).vault.azure.net/secrets/$($messageBrokerName)')"
	$exists = az keyvault secret list --vault-name $keyVaultName --query $query
	if ($exists -eq "false") {
		az keyvault secret set --vault-name $keyVaultName --name $messageBrokerName --value "$($messageBrokerConnectionString)" --output none
		Write-Host "##[section]Set secret $messageBrokerName"
	}
}

if ($appInsightsConnectionString -ne $null) {
	$query = "contains([].id, 'https://$($keyVaultName).vault.azure.net/secrets/$($applicationInsightsConnectionName)')"
	$exists = az keyvault secret list --vault-name $keyVaultName --query $query
	if ($exists -eq "false") {
		az keyvault secret set --vault-name $keyVaultName --name $applicationInsightsConnectionName --value "$($appInsightsConnectionString)" --output none
		Write-Host "##[section]Set secret $applicationInsightsConnectionName"
	}
}
