# Working with the template

It is important to keep the `.template.config` folder where it is. `.nuspec` file also must be kept in the same location as it is now.

## Commands

When making changes to the template, execute the commands below. Don't forget to bump version in `/source/.template.config/template.json` and `/SparkRoseDigital.Template.nuspec` (this is done by the pipeline). Now, first you need to download `nuget.exe` to a folder above the template folder. Then start executing below commands.

Package the template:

    ./nuget.exe pack ./<template_folder>/SparkRoseDigital.Template.nuspec -OutputDirectory ./SparkRoseDigital.Template.NugetPackage/nupkg -NoDefaultExcludes

Install from the local NuGet package file (do this only for testing purposes):

    dotnet new --install ./SparkRoseDigital.Template.NugetPackage/nupkg/SparkRoseDigital.Template.<version>.nupkg

Push to Nuget (find the api key in the vault):

    dotnet nuget push ./SparkRoseDigital.Template.NugetPackage/nupkg/SparkRoseDigital.Template.<version>.nupkg --api-key <api_key> --source https://api.nuget.org/v3/index.json

Install the template from NuGet feed (get the below command from NuGet template entry):

    dotnet new --install SparkRoseDigital.Template
