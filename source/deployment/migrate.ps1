param ($locationExecutableDll, $dbConnection, $sqlAdminUsername, $sqlAdminPassword, $adoDropFolder, $usersGroupName)

$PSNativeCommandUseErrorActionPreference = $true

dotnet "$($locationExecutableDll)" $dbConnection $sqlAdminUsername $sqlAdminPassword $adoDropFolder $usersGroupName