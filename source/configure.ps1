Write-Output "This script will set up the git init (if not done already) and initialize githooks."
Write-Output "Please enter following information to configure SparkRoseDigital_Template application for local use."
Write-Output "Values you provide here will be bound to .env file."
Write-Output "Default values are provided for usernames and passwords, but you can enter a different value if you like."
Write-Output "Some inputs do not have default values, you will probably have to get these yourself from external systems (Azure)."
Write-Output "You can rerun the script but no new values will be applied to the .env file."
Write-Output "If you want to edit a previously provided value, it is best to edit .env file manually."

# If none, create a ".env" file
if (!(Test-Path ".env"))
{
   New-Item -name ".env" -type "file" -value @"
APPLICATIONINSIGHTS_CONNECTION_STRING=<applicationinsights_connection_string>
ConnectionStrings__SparkRoseDigital_TemplateDbConnection=Data Source=sparkrosedigital_template.sql;Initial Catalog=SparkRoseDigital_TemplateDb
ConnectionStrings__SparkRoseDigital_TemplateDb_Migrations_Connection=Data Source=host.docker.internal,1433;Initial Catalog=SparkRoseDigital_TemplateDb;Encrypt=False
ConnectionStrings__MessageBroker=<msg_broker_connection_string>
DB_USER=<db_user>
DB_PASSWORD=<db_pw>
DB_ADMIN_PASSWORD=<db_admin_pw>
AUTH__AUTHORITY=<auth_authority>
AUTH__AUDIENCE=<auth_audience>
AUTH__VALID_ISSUER=<auth_valid_issuer>
"@
    Write-Host "Created new file and text content added"
}

# If none, create a ".variables.ps1" file
if (!(Test-Path "deployment/variables.ps1"))
{
   New-Item -name "deployment/variables.ps1" -type "file" -value @"
# Used only for LOCAL deployment!
$SUBSCRIPTION=""
$LOCATION=""
$ENVIRONMENT=""
$PROJECT_NAME=""
$DB_USER=""
$DB_PASSWORD=""
"@
    Write-Host "Created new file and text content added"
}

# Database administrator password
$db_admin_pw_default = "Pa55w0rd_1337"
if (!($db_admin_pw = Read-Host "Database admin password [$db_admin_pw_default]")) { $db_admin_pw = $db_admin_pw_default }
# Database username
$db_user_default = "SparkRoseDigital_TemplateDb_Login"
if (!($db_user = Read-Host "Database user name [$db_user_default]")) { $db_user = $db_user_default }
# Database password
$db_pw_default = 'Pa55w0rd_1337'
if (!($db_pw = Read-Host "Database user password [$db_pw_default]")) { $db_pw = $db_pw_default }
# Message broker connection string
$msg_broker_connection_string = Read-Host -Prompt 'Message broker connection string (Azure Service Bus)'
# Azure Application Insights Connection String
$applicationinsights_connection_string = Read-Host -Prompt 'Application Insights connection string (Azure)'
# Azure AD Authority URL
$auth_authority = Read-Host -Prompt 'Azure AD Authority URL'
# Claim identifying this API
$auth_audience = Read-Host -Prompt 'This APIs audience identifier'
# Valid issuer (since AAD authority URL isn't the same as what is in the issued JWT tokens)
$auth_valid_issuer = Read-Host -Prompt 'Azure AD Valid Issuer'

(Get-Content ".env").replace("<db_admin_pw>", $db_admin_pw) | Set-Content ".env"
(Get-Content ".env").replace("<db_user>", $db_user) | Set-Content ".env"
(Get-Content ".env").replace("<db_pw>", $db_pw) | Set-Content ".env"
(Get-Content ".env").replace("<msg_broker_connection_string>", $msg_broker_connection_string) | Set-Content ".env"
(Get-Content ".env").replace("<applicationinsights_connection_string>", $applicationinsights_connection_string) | Set-Content ".env"
(Get-Content ".env").replace("<auth_authority>", $auth_authority) | Set-Content ".env"
(Get-Content ".env").replace("<auth_audience>", $auth_audience) | Set-Content ".env"
(Get-Content ".env").replace("<auth_valid_issuer>", $auth_valid_issuer) | Set-Content ".env"

# git init only on a new repo
git rev-parse --is-inside-work-tree | Out-Null
if ( $LASTEXITCODE -ne 0)
{
    git init
    git add .gitignore
    git commit -m "gitignore"
}

git config core.hooksPath "./githooks"