
# EF 7

-- The DB Context represents the whole database
-- The DBSet Represent a Table of View

## Packages to install
-- For DbContext Microsoft.EntityFrameworkCore (For basic functionality like DBContext)
-- Microsoft.EntityFrameworkCore.Design (For EF Design First)
-- Microsoft.EntityFrameworkCore.SqlServer for Server connection
-- Microsoft.EntityFrameworkCore.Tools for dotnet ef commandline

## Running MYSQL Server on Docker

```bash
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

# Database constraints and defaults using Fluent API in DBCOntext class onModelCreating

'''Bash

 protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Country>().ToTable("Countries");
			modelBuilder.Entity<Person>().ToTable("Persons");


			// Data Seeding

			// Reading the jsonfiles

			var CountryText = System.IO.File.ReadAllText("./sampleData/countries.json");
			var CountryArray = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(CountryText);

            foreach (Country item in CountryArray!)
			{
                modelBuilder.Entity<Country>().HasData(item);
            }


            var PersonsText = System.IO.File.ReadAllText("./sampleData/persons.json");
            var PersonsList = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(PersonsText);

            foreach (Person item in PersonsList!)
            {
                modelBuilder.Entity<Person>().HasData(item);
            }

            // Using Fluent API

            modelBuilder.Entity<Person>().Property(property => property.TIN)
                .HasColumnName("TaxIdentificationNumber")
                .HasColumnType("varchar(10)")
                .HasDefaultValue("ABISD882");

            // Using Fluent API To Create Index

            modelBuilder.Entity<Country>().HasIndex(item => item.CountryName).IsUnique();


            // Using Fluent Constraint
           // modelBuilder.Entity<Person>().ToTable(t => t.HasCheckConstraint("CHECK_TIN_LENGHTH", "len([TaxIdentificationNumber])=10"));
            // modelBuilder.Entity<Person>().HasCheckConstraint("CHECK_TIN_LENGHTH", "len([TaxIdentificationNumber])=10");
        }
'''


## EF Database Modeling




## Database Testing using Moq Framework

* install Moq framework
* install EntityFramworkCoreMock.Moq

## Testing dependancy

   <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.5.0" />
    <PackageReference Include="xunit" Version="2.4.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.4.5">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="coverlet.collector" Version="3.2.0">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="Moq" Version="4.20.69" />
    <PackageReference Include="EntityFrameworkCoreMock.Moq" Version="2.4.0" />
    <PackageReference Include="AutoFixture" Version="4.18.0" />