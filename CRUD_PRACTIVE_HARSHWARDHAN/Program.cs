using Microsoft.EntityFrameworkCore;
using Entity;
using Services;
using ServiceContracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Adding service as dependency
builder.Services.AddScoped<IPersonsService,PersonsService>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
// Adding Entitiy Framework Context

builder.Services.AddDbContext<PesonsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

