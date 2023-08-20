param resourceGroup_name string
param resourceGroup_location string

targetScope = 'subscription'

resource resourceGroup_resource 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: resourceGroup_name
  location: resourceGroup_location
}
