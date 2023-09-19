# Use this file only to provision Azure resources from local.

az account set -s $SUBSCRIPTION

# Resource group
$jsonResultRg = az deployment sub create --location $LOCATION --template-file ./deployment/resource-group.bicep --parameters environment=$ENVIRONMENT projectName=$PROJECT_NAME location=$LOCATION | ConvertFrom-Json
$resourceGroupName = $jsonResultRg.properties.outputs.resourceGroupName.value

# Resources
az deployment group create --resource-group $resourceGroupName --template-file .\deployment\iac.bicep --parameters environment=$ENVIRONMENT projectName=$PROJECT_NAME db_user=$DB_USER db_password=$DB_PASSWORD