# Before You Get Started

## Install a Docker host

E.g. Docker Desktop:

    choco install docker-desktop

## Configure Azure Service Bus

Create a new namespace, with two Shared access policies, one for reading (called "ReaderPolicy") and one for writing (called "WriterPolicy"). Make sure both have `Manage` permission. Now find the primary connection string and copy it somewhere. You will only need the part up until the first semicolon (`Endpoint=sb://whatever.servicebus.windows.net/`). Also make a note of your policies names and keys. You will need all of these to configure the environment variables.

## Set environment variables

`.env` file example:

    ConnectionStrings__MessageBroker=
    MessageBroker__Writer__SharedAccessKeyName=
    MessageBroker__Writer__SharedAccessKey=
    MessageBroker__Reader__SharedAccessKeyName=
    MessageBroker__Reader__SharedAccessKey=
    EmailSettings__Username=
    EmailSettings__Password=
    DB_USER=
    DB_PASSWORD=

# Running The Application

Make sure to set the `docker-compose` as the startup project. The application can be reached by default on `localhost:44395`. You can change this in the `docker-compose.yml`. Just go to `/swagger/index.html` to see the initial API.

At this point you have several things up and running:

- API
- Worker service
- Empty PostgreSQL database.
- Azure Service bus with several topics, subscriptions and queues.

Now it is time to create some tables in the database. From the root of your solution, first run `.\create_migration.ps1 '' '0001_Initial'` and then `./migrate.ps1`. Now you have to go to the PgAdmin and register your database server there. It is accessible on localhost, port 5432, with the password you set in your `.env` file.

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
