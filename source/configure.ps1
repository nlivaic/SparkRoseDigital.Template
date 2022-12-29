Write-Output "Please enter following information to configure SparkRoseDigital_Template application."
Write-Output "Values you provide here will be bound to .env file."
Write-Output "Default values are provided for usernames and passwords, but you can enter a different value if you like."
Write-Output "Some inputs do not have default values, you will probably have to get these yourself from external systems."
Write-Output "You can rerun the script but only the values you are entering for the first time will be applied to the .env file."
Write-Output "If you want to edit a previously provided value, it is best to edit .env file manually."

# Smtp service username
$smtp_user_default = "smtp_user"
if (!($smtp_user = Read-Host "Smtp service username [$smtp_user_default]")) { $smtp_user = $smtp_user_default }
# Smtp service password
$smtp_pw_default = "smtp_password"
if (!($smtp_pw = Read-Host "Smtp service password [$smtp_pw_default]")) { $smtp_pw = $smtp_pw_default }
# Database administrator password
$db_admin_pw_default = "rootpw_ROOTPW_1"
if (!($db_admin_pw = Read-Host "Database admin password [$db_admin_pw_default]")) { $db_admin_pw = $db_admin_pw_default }
# Database username
$db_user_default = "SparkRoseDigital_TemplateDb_Login"
if (!($db_user = Read-Host "Database user name [$db_user_default]")) { $db_user = $db_user_default }
# Database password
$db_pw_default = 'Pa55w0rd_1337'
if (!($db_pw = Read-Host "Database user password [$db_pw_default]")) { $db_pw = $db_pw_default }
# Message broker connection string
$msg_broker_connection_string = Read-Host -Prompt 'Message broker connection string (Azure Service Bus)'
# Message broker read policy
$msg_broker_read_policy = Read-Host -Prompt 'Message broker read policy (Azure Service Bus)'
#Message broker write policy
$msg_broker_write_policy = Read-Host -Prompt 'Message broker write policy (Azure Service Bus)'

if (![string]::IsNullOrWhiteSpace($smtp_user)) {
    (Get-Content ".env").replace("<smtp_user>", $smtp_user) | Set-Content ".env"
}
if (![string]::IsNullOrWhiteSpace($smtp_pw)) {
    (Get-Content ".env").replace("<smtp_pw>", $smtp_pw) | Set-Content ".env"
}
if (![string]::IsNullOrWhiteSpace($db_admin_pw)) {
    (Get-Content ".env").replace("<db_admin_pw>", $db_admin_pw) | Set-Content ".env"
}
if (![string]::IsNullOrWhiteSpace($db_user)) {
    (Get-Content ".env").replace("<db_user>", $db_user) | Set-Content ".env"
}
if (![string]::IsNullOrWhiteSpace($db_pw)) {
    (Get-Content ".env").replace("<db_pw>", $db_pw) | Set-Content ".env"
}
if (![string]::IsNullOrWhiteSpace($msg_broker_connection_string)) {
    (Get-Content ".env").replace("<msg_broker_connection_string>", $msg_broker_connection_string) | Set-Content ".env"
}
if (![string]::IsNullOrWhiteSpace($msg_broker_read_policy)) {
    (Get-Content ".env").replace("<msg_broker_read_policy>", $msg_broker_read_policy) | Set-Content ".env"
}
if (![string]::IsNullOrWhiteSpace($msg_broker_write_policy)) {
    (Get-Content ".env").replace("<msg_broker_write_policy>", $msg_broker_write_policy) | Set-Content ".env"
}

git config core.hooksPath "./githooks"