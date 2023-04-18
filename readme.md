# Working with the template

It is important to keep the `.template.config` folder where it is. `.nuspec` file also must be kept in the same location as it is now.

Make sure to execute `./configure_template.ps1` from the root folder of the template after cloning the repository to a local folder. This will wire up stuff in the `templategithooks` folder.

## Branching strategy

Feature branches strategy is supported out of the box. This strategy expects all development to go through branches and committing directly to `master` is not allowed. Supported branches:

* `feature/`
* `fix/`

## Pipelines

There are three Azure YAML pipelines:

* `sparkrosedigital_template-pr_pipeline`
* `sparkrosedigital_template-build_pipeline`
* `sparkrosedigital_template-release_pipeline`

All pipelines build and deploy all applications (`Api` and `WorkerServices`) in the solution.

When creating ADO pipelines, name them just like the files are named (minus the `.yml` suffix).

`template_release_pipeline.yml` - `project` on line 42 should be the name of your ADO project.

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
