using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Entity
{
	public class PesonsDbContext : DbContext
	{
		public virtual DbSet<Person> Persons { get; set; }

		public virtual DbSet<Country> Countries { get; set; }

		public PesonsDbContext(DbContextOptions<PesonsDbContext> options) : base(options)
		{
		}

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

        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[] {
            new SqlParameter("@PersonID", person.PersonID),
            new SqlParameter("@PersonName", person.PersonName),
            new SqlParameter("@Email", person.Email),
            new SqlParameter("@DateOfBirth", person.DateOfBirth),
            new SqlParameter("@Gender", person.Gender),
            new SqlParameter("@CountryID", person.CountryID),
            new SqlParameter("@Address", person.Address),
            new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)
          };

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetters", parameters);
        }
    }
}

