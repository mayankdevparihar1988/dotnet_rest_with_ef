using System;
using Microsoft.EntityFrameworkCore;

namespace Entity
{
	public class PesonsDbContext : DbContext
	{
		public DbSet<Person> Persons { get; set; }

		public DbSet<Country> Countries { get; set; }

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

            foreach (Country item in CountryArray)
			{
                modelBuilder.Entity<Country>().HasData(item);
            }


            var PersonsText = System.IO.File.ReadAllText("./sampleData/persons.json");
            var PersonsList = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(PersonsText);

            foreach (Person item in PersonsList)
            {
                modelBuilder.Entity<Person>().HasData(item);
            }

        }
    }
}

