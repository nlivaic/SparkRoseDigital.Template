# Before You Get Started

## Install a Docker host

E.g. Docker Desktop:

    choco install docker-desktop

## Configure Azure Service Bus

Create a new namespace, with two Shared access policies, one for reading (called "ReaderPolicy") and one for writing (called "WriterPolicy"). Make sure both have `Manage` permission. Now find the primary connection string and copy it somewhere. You will only need the part up until the first semicolon (`Endpoint=sb://yourApplicationName.servicebus.windows.net/`). Also make a note of your policies names and keys. You will need all of these to configure the environment variables.

## Configuration

### Set configuration

Most of the stuff is in the `.env` file. This is a git ignored file, but it has the relevant structure in it. Some of the fields are prepopulated, others have to be provided by you:

- Connection strings and details for the Service Bus (details above).
- Username and password for the email are provided as default values, but you can provide whatever values you want.

### Database configuration

Database connection string for both `Api`, `WorkerServices` and `Migrations` projects is in the `.env` file. This was a deliberate choice, because I wanted the templated project to have a connection string automatically generated and in line with the name of the solution. You will notice there are two connection strings: `ConnectionStrings__SparkRoseDigital_TemplateDbConnection` is used by `Api` and `WorkerServices`. `Migrations` has a separate one `ConnectionStrings__SparkRoseDigital_TemplateDb_Migrations_Connection` because it is accessing the dockerized database from outside.

Username and password for the database are provided as default values, but you can provide whatever values you want.
Make sure you set the database-related variables (prefixed `DB_`) before you run the solution for the first time, otherwise the database will be configured with given administrator password and a username and password for the application user. If you don't change those values before running the solution you will have to delete the `sparkrosedigital_template.sql` container and accompanying volumes. If you change `DB_PASSWORD`, make sure the same value is set in `InitializeSparkRoseDigital_TemplateDb.sql` for the login as well.

When you first run the solution, an SQL script found in `src/InitializeSparkRoseDigital_TemplateDb.sql` is executed, creating the database with an admin account (password in `DB_ADMIN_PASSWORD`), login and user (`DB_USER` and `DB_PASSWORD`). User is then assigned to read, write and DDL roles.

Application is accessing the database as a `DB_USER`/`DB_PASSWORD`, with a generated connection string found in `ConnectionStrings__SparkRoseDigital_TemplateDbConnection` and `ConnectionStrings__SparkRoseDigital_TemplateDb_Migrations_Connection`.

# Running The Application

Make sure to set the `docker-compose` as the startup project. The application can be reached by default on `localhost:44395`. You can change this in the `docker-compose.yml`. Just go to `/swagger/index.html` to see the initial API.

At this point you have several things up and running:

- API
- Worker service
- Empty Sql Server database.
- Azure Service bus with several topics, subscriptions and queues.
- nginx reverse proxy
- Smtp fake email server
- Seq log management service

Now it is time to create some tables in the database. From the root of your solution, first run `.\create_migration.ps1 '' '0001_Initial'` and then `./migrate.ps1`. Now you have to go to the SSMS and register your database server there. It is accessible on localhost, port 1433, with the username and password you set in your `.env` file under `DB_USER` and `DB_PASSWORD`.

# Additional Stuff

## Generating cert for your local development box

The template does not use HTTPS, however it can easily be added. There is a `.conf` file in there which you need to tweak to your liking. Then you need to generate `.crt` and `.key` files for Api. These make up the self-signed certificate, and the commands to create the certificate are below, with a dummy password of `rootpw`:

1. Go to **solution root folder** and execute below lines from **WSL2**:

   sudo openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout api-local.sparkrosedigital_template.key -out api-local.sparkrosedigital_template.crt -config api-local.sparkrosedigital_template.conf -passin pass:rootpw

   sudo openssl pkcs12 -export -out api-local.sparkrosedigital_template.pfx -inkey api-local.sparkrosedigital_template.key -in api-local.sparkrosedigital_template.crt

2. Add the certificate to your computers CA store: go to ./nginx, right-click on `.pfx` files and install to `LocalMachine` -> `Trusted Root Certification Authorities`.

For more details consult: https://bit.ly/3eWOHH2

## Hosts file

You can tell nginx to work with the `localhost`, however this might become a problem if you have multiple services running. To sidestep the issue you can keep the nginx.conf as it is, but that will require a change to `hosts` file.

    # Development DNS
    127.0.0.1	    api-local.sparkrosedigital_template.com
    127.0.0.1	    id-local.sparkrosedigital_template.com

## Migrations

For migrations to work `.env` file must be properly set up with database credentials and connection string configured.

### Creating migrations

The below commands must be executed from solution root folder using Powershell. If this is the first migration in your project, execute:

    .\create_migration.ps1 '' '0001_Initial'

Every next migration must contain the name of the migration immediately preceeding it:

    .\create_migration.ps1 '0001_Initial' '0002_Second'

### Applying migrations

Command must be executed from solution root folder using Powershell. You will notice it is executing from a Docker container and Docker compose - the reason is this way there is only one `.env` which can be shared by all executeable projects in the solution (`Ąpi`, `Migrations`, `WorkerServices`).

    ./migrate.ps1
