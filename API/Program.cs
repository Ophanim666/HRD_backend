using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Models.Entidades;
using API;
using Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios en el contenedor.
builder.Services.AddControllers();

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(Automapping));

// Configurar la cadena de conexi�n
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


//ProveedorRepository
builder.Services.AddScoped<ProveedorRepository>(provider => new ProveedorRepository(connectionString));

// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline de solicitud HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//esto lo ponemos aqui pq debe ir antes de la autorizacion
app.UseCors(x => x.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   );

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();