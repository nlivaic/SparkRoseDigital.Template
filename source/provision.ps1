# Read from variables.ps1 only as part of local development. Detect local environment by trying 
# to read from "rg" environment variable. If "rg" environment variable is not defined, 
# deduce there aren't any relevant environment variables defined and read from variables.ps1.
# Pipeline should have "rg" and all the other relevant variables defined, so no need for variables.ps1
if ($RG -like '') {
    . .\deployment\variables.ps1
}

echo $RG

az account set -s $SUBSCRIPTION

# Resource group
az deployment sub create --location $LOCATION --template-file .\deployment\resource-group.bicep --parameters resourceGroup_name=$RG resourceGroup_location=$LOCATION

# App Service Plan and App Service
az deployment group create --resource-group $RG --template-file .\deployment\app-service-plan.bicep --parameters appServicePlan_name=$APP_SERVICE_PLAN_NAME appServicePlan_location=$LOCATION appService_web_name=$APP_SERVICE_WEB_NAME

# Sql Server and Sql Database
az deployment group create --resource-group $RG --template-file .\deployment\sql-server.bicep --parameters sqlserver_name=$SQLSERVER_NAME sqlserver_location=$LOCATION sqlserver_administratorLoginPassword=$SQLSERVER_ADMINISTRATOR_LOGIN_PASSWORD sqlserver_administratorLogin=$SQLSERVER_ADMINISTRATOR_LOGIN sqldb_name=$SQLDB_NAME

# Application Insights

# Service Bus
