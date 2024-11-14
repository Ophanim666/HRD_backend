using Data.Repositorios;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using DTO.Usuario;
using Models.Entidades;
using API;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios en el contenedor.
builder.Services.AddControllers(); // Llamada a AddControllers, solo la primera vez

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
//Acta
builder.Services.AddScoped<ActaRepository>(provider => new ActaRepository(connectionString));
//ProveedorRepository
builder.Services.AddScoped<ProveedorRepository>(provider => new ProveedorRepository(connectionString));
// Registrar TareaRepository
builder.Services.AddScoped<TareaRepository>(provider => new TareaRepository(connectionString));

// ---------------------------------------------
// Configuración de JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

// Configurar la autenticación con JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Para desarrollo local
    options.SaveToken = true; // Almacenar el token en las solicitudes
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
        ClockSkew = TimeSpan.Zero // Ajuste de tiempo entre la expiración del token
    };
});
// ---------------------------------------------

// Agregar políticas de autorización
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireClaim("es_administrador", "True")); // El claim debe ser "True" para que sea considerado administrador
});

// Configurar el TokenService
builder.Services.AddSingleton<TokenService>(provider => new TokenService(
    builder.Configuration["JwtSettings:SecretKey"],
    builder.Configuration["JwtSettings:Issuer"],
    builder.Configuration["JwtSettings:Audience"]
));

//ProveedorRepository
builder.Services.AddScoped<ProveedorRepository>(provider => new ProveedorRepository(connectionString));


//Obra
builder.Services.AddScoped<ObraRepository>(provider => new ObraRepository(connectionString));


// Configurar Swagger
builder.Services.AddEndpointsApiExplorer();
//verificar si esta autentificacion esta bien empleada
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    // Configuración para la autenticación Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor ingresa el token JWT en el formato **Bearer {token}**",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    // Agregar requisito de seguridad
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


var app = builder.Build();

// Configurar el pipeline de solicitud HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuración de CORS
app.UseCors(x => x.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod());

app.UseHttpsRedirection();

app.UseRouting();

// Agregar el middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
