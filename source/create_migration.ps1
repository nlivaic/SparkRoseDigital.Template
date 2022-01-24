# Usage: migrations_su.ps1 <previous_migration> <next_migration_number> <next_migration_name>
# Usage: migrations_su.ps1 '0001_Initial' '0003_StudycastIntegration'
$previous_migration=$args[0]
$next_migration_name=$args[1]
$full_script_path="../SparkRoseDigital_Template.Migrations/SparkRoseDigital_TemplateScripts/" + $next_migration_name + ".sql"
cd ./src/SparkRoseDigital_Template.Data
dotnet ef migrations add $next_migration_name --startup-project ../SparkRoseDigital_Template.Api/SparkRoseDigital_Template.Api.csproj --context SparkRoseDigital_TemplateDbContext
if ($previous_migration -eq '')
{
    dotnet ef migrations script --startup-project ../SparkRoseDigital_Template.Api/SparkRoseDigital_Template.Api.csproj --context SparkRoseDigital_TemplateDbContext -o $full_script_path
}
else
{
    dotnet ef migrations script --startup-project ../SparkRoseDigital_Template.Api/SparkRoseDigital_Template.Api.csproj --context SparkRoseDigital_TemplateDbContext $previous_migration $next_migration_name -o $full_script_path
}
cd ../..