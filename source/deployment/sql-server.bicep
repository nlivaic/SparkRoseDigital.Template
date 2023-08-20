param sqlserver_name string
param sqlserver_location string
param sqlserver_administratorLogin string
@secure()
param sqlserver_administratorLoginPassword string
param sqldb_name string

resource sqlserver 'Microsoft.Sql/servers@2022-11-01-preview' = {
  name: sqlserver_name
  location: sqlserver_location
  properties: {
    administratorLogin: sqlserver_administratorLogin
    administratorLoginPassword: sqlserver_administratorLoginPassword
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    restrictOutboundNetworkAccess: 'Disabled'
  }
}

resource sqlserver_AllowAllWindowsAzureIps 'Microsoft.Sql/servers/firewallRules@2022-11-01-preview' = {
  parent: sqlserver
  name: 'AllowAllWindowsAzureIps'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource sqlDb 'Microsoft.Sql/servers/databases@2022-05-01-preview' = {
  parent: sqlserver
  name: sqldb_name
  location: sqlserver_location
  sku: {
    name: 'Basic'
    tier: 'Basic'
    capacity: 5
  }
}
