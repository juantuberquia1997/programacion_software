using apiFestRock.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BdEventosContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cnx"));
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opciones => {
    opciones.AddPolicy("politicaGral", app => {
        app.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI();
}


app.UseCors("politicaGral");

app.UseAuthorization();

app.MapControllers();

app.Run();
