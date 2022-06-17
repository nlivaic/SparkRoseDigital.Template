# Getting Started

### Install a Docker host

E.g. Docker Desktop:

    choco install docker-desktop

### Set environment variables

`.env` file example:

    MessageBroker__Writer__SharedAccessKeyName=
    MessageBroker__Writer__SharedAccessKey=
    MessageBroker__Reader__SharedAccessKeyName=
    MessageBroker__Reader__SharedAccessKey=
    EmailSettings__Username=
    EmailSettings__Password=
    DB_USER=
    DB_PASSWORD=

### Generating cert for your local development box

The template does not use HTTPS, however it can easily be added. There is a `.conf` file in there which you need to tweak to your liking. Then you need to generate `.crt` and `.key` files for Api. These make up the self-signed certificate, and the commands to create the certificate are below, with a dummy password of `rootpw`:

1. Go to **solution root folder** and execute below lines from **WSL2**:

   sudo openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout api-local.sparkrosedigital_template.key -out api-local.sparkrosedigital_template.crt -config api-local.sparkrosedigital_template.conf -passin pass:rootpw
   sudo openssl pkcs12 -export -out api-local.sparkrosedigital_template.pfx -inkey api-local.sparkrosedigital_template.key -in api-local.sparkrosedigital_template.crt

2. Add the certificate to your computers CA store: go to ./nginx, right-click on `.pfx` files and install to `LocalMachine` -> `Trusted Root Certification Authorities`.

For more details consult: https://bit.ly/3eWOHH2

### Hosts file

You can tell nginx to work with the `localhost`, however this might become a problem if you have multiple services running. To sidestep the issue you can keep the nginx.conf as it is, but that will require a change to `hosts` file.

    # Development DNS
    127.0.0.1	    api-local.sparkrosedigital_template.com
    127.0.0.1	    id-local.sparkrosedigital_template.com

# Migrations

For migrations to work `.env` file must be properly set up with database credentials and the `Migrations` project's `appsettings.json` / `appsettings.Development.json` must have the connection string configured.

### Creating migrations

The below commands must be executed from solution root folder using Powershell. If this is the first migration in your project, execute:

    .\create_migration.ps1 '' '0001_Initial'

Every next migration must contain the name of the migration immediately preceeding it:

    .\create_migration.ps1 '0001_Initial' '0002_Second'

### Applying migrations

Command must be executed from solution root folder using Powershell. You will notice it is executing from a Docker container and Docker compose - the reason is this way there is only one `.env` which can be shared by all executeable projects in the solution (`Ąpi`, `Migrations`, `WorkerServices`).

    ./migrate.ps1

# Working with the template

It is important to keep the `.template.config` folder where it is. `.nuspec` file also must be kept in the same location as it is now.

## Commands

When making changes to the template, execute the commands below. Don't forget to bump version in `/source/.template.config/template.json` and `/SparkRoseDigital.Template.nuspec`. Now, first you need to download `nuget.exe` to a folder above the template folder. Then start executing below commands.

Package the template:

    ./nuget.exe pack ./<template_folder>/SparkRoseDigital.Template.nuspec -OutputDirectory ./SparkRoseDigital.Template.NugetPackage/nupkg -NoDefaultExcludes

Install from the local NuGet package file (do this only for testing purposes):

    dotnet new --install ./SparkRoseDigital.Template.NugetPackage/nupkg/SparkRoseDigital.Template.<version>.nupkg

Push to Nuget (find the api key in the vault):

    dotnet nuget push ./SparkRoseDigital.Template.NugetPackage/nupkg/SparkRoseDigital.Template.<version>.nupkg --api-key <api_key> --source https://api.nuget.org/v3/index.json

Install the template from NuGet feed (get the below command from NuGet template entry):

    dotnet new --install SparkRoseDigital.Template::<version>
