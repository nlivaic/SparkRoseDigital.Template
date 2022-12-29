# Working with the template

It is important to keep the `.template.config` folder where it is. `.nuspec` file also must be kept in the same location as it is now.

# After cloning

Make sure to execute `./configure_template.ps1` from the root folder of the template. This will wire up stuff in the `templategithooks` folder.

## Commands

When making changes to the template, just push to GitHub and Azure Yaml pipelines will take care of building and deploying to NuGet. Version is bumped by the pipeline, but earlier the developer had to do it manually in `/source/.template.config/template.json` and `/SparkRoseDigital.Template.nuspec`. Next thing you have to do is download `nuget.exe` to a folder above the template folder.

### Package the template

Below command is not needed anymore as the pipeline does everything, but they are still here for the sake of completeness and documentation.

    ./nuget.exe pack ./<template_folder>/SparkRoseDigital.Template.nuspec -OutputDirectory ./SparkRoseDigital.Template.NugetPackage/nupkg -NoDefaultExcludes

### Install from the local NuGet package file (do this only for testing purposes):

    dotnet new --install ./SparkRoseDigital.Template.NugetPackage/nupkg/SparkRoseDigital.Template.<version>.nupkg

### Push to Nuget (find the api key in the vault)

Below command is not needed anymore as the pipeline does everything, but they are still here for the sake of completeness and documentation.

    dotnet nuget push ./SparkRoseDigital.Template.NugetPackage/nupkg/SparkRoseDigital.Template.<version>.nupkg --api-key <api_key> --source https://api.nuget.org/v3/index.json

## Install the template from NuGet feed (get the below command from NuGet template entry):

You still have to execute this command:

    dotnet new --install SparkRoseDigital.Template
