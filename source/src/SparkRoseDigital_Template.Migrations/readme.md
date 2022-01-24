#### How to migrate the database

##### Development

No need to do anything. When the solution is ran Docker will run both the database container and 
DbUp migration container. Migration container will wait for the database to spin up and then 
will execute the migrations.

##### Production

Configure connection string using CLI arguments. First argument should be a string with the entire
connection string.