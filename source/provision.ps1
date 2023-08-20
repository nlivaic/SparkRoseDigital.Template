# Read from variables.ps1 only as part of local development. Detect local environment by trying 
# to read from "rg" environment variable. If "rg" environment variable is not defined, 
# deduce there aren't any relevant environment variables defined and read from variables.ps1.
# Pipeline should have "rg" and all the other relevant variables defined, so no need for variables.ps1
if ($rg -like '') {
    . .\deployment\variables.ps1
}

echo $rg

az account set -s $subscription

# Resource group
az deployment sub create --location $location --template-file .\deployment\resource-group.bicep --parameters resourceGroup_name=$rg resourceGroup_location=$location

# App Service Plan and App Service
az deployment group create --resource-group $rg --template-file .\deployment\app-service-plan.bicep --parameters appServicePlan_name=$app_service_plan_name appServicePlan_location=$location appService_web_name=$app_service_web_name

# Sql Server and Sql Database
az deployment group create --resource-group $rg --template-file .\deployment\sql-server.bicep --parameters sqlserver_name=$sqlserver_name sqlserver_location=$location sqlserver_administratorLoginPassword=$sqlserver_administratorLoginPassword sqlserver_administratorLogin=$sqlserver_administratorLogin sqldb_name=$sqldb_name

# Application Insights

# Service Bus
