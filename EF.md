
# EF 7

-- The DB Context represents the whole database
-- The DBSet Represent a Table of View

## Packages to install
-- For DbContext Microsoft.EntityFrameworkCore (For basic functionality like DBContext)
-- Microsoft.EntityFrameworkCore.Design (For EF Design First)
-- Microsoft.EntityFrameworkCore.SqlServer for Server connection
-- Microsoft.EntityFrameworkCore.Tools for dotnet ef commandline

## Running MYSQL Server on Docker

```
 docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=YourPassword123' -p 1433:1433 --name sql_server_container -d mcr.microsoft.com/mssql/server
 ``

## Connection String for Dockerized SQL



## DB Migration using dotnet cli
Recently able to generate migrations using donet ef cli

using the following command.

ef cli using dotnet tool

++ Example
Command need to be executed where DBContext is defiend
Migration command:::: 

'''bash dotnet ef migrations add InitalDbMigration --startup-project ../CRUD_PRACTIVE_HARSHWARDHAN ''



Deployment Command:::

 '''bash  dotnet ef database update --startup-project ../CRUD_PRACTIVE_HARSHWARDHAN ''''


 mayankparihar@Mayanks-MacBook-Pro-2 Entity % dotnet ef migrations add InitialDbMigration --startup-project ../CRUD_PRACTIVE_HARSHWARDHAN
Build started...
Build succeeded.
Done. To undo this action, use 'ef migrations remove'
mayankparihar@Mayanks-MacBook-Pro-2 Entity % dotnet ef migrations database update  --startup-project ../CRUD_PRACTIVE_HARSHWARDHAN
Build started...
Build succeeded.
Specify --help for a list of available options and commands.
Unrecognized command or argument 'database'
mayankparihar@Mayanks-MacBook-Pro-2 Entity % dotnet ef database update  --startup-project ../CRUD_PRACTIVE_HARSHWARDHAN 