using Data.Repositorios;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using DTO.Usuario;
using Models.Entidades;
using API;
using Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios en el contenedor.
builder.Services.AddControllers();

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(Automapping));

// Configurar la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<UsuarioRepository>(provider => new UsuarioRepository(connectionString));
//Especialidad
builder.Services.AddScoped<EspecialidadRepository>(provider => new EspecialidadRepository(connectionString));
//TipoParametro
builder.Services.AddScoped<TipoParametroRepository>(provider => new TipoParametroRepository(connectionString));
//Parametro
builder.Services.AddScoped<ParametroRepository>(provider => new ParametroRepository(connectionString));




//ProveedorRepository
builder.Services.AddScoped<ProveedorRepository>(provider => new ProveedorRepository(connectionString));

// Registrar TareaRepository
builder.Services.AddScoped<TareaRepository>(provider => new TareaRepository(connectionString));

//Obra
builder.Services.AddScoped<ObraRepository>(provider => new ObraRepository(connectionString));


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

// Esto lo ponemos aqui pq debe ir antes de la autorizacion
app.UseCors(x => x.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   );

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();