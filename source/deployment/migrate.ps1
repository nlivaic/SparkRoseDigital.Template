# Connection string defaults to an environment variable because it contains ';',
# which Azure DevOps shell task arguments validation rejects when passed via task arguments.
param ($locationExecutableDll, $dbConnection = $env:DB_CONNECTION_STRING, $sqlAdminUsername, $sqlAdminPassword, $adoDropFolder, $usersGroupName)

$PSNativeCommandUseErrorActionPreference = $true

dotnet "$($locationExecutableDll)" $dbConnection $sqlAdminUsername $sqlAdminPassword $adoDropFolder $usersGroupName