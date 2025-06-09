using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using passwordManagent2.Models;

using Microsoft.EntityFrameworkCore.Sqlite;


using BenchmarkDotNet.Running;
using passwordManagent2.Benchmark; // Asegúrate de que este namespace coincida con el de tu clase Benchmark

var summary = BenchmarkRunner.Run<BenchmarkClass>();

// Para debug: prueba manual
#if DEBUG
Console.WriteLine("=== PRUEBA MANUAL ===");
var service = new passwordManagent2.services.EncryptionPassword();
var encrypted = service.Encrypt("200211", "200211");
Console.WriteLine($"Encrypted: {encrypted}");
var decrypted = service.Decrypt(encrypted, "200211");
Console.WriteLine($"Decrypted: {decrypted}");
#endif

var builder = WebApplication.CreateBuilder(args);


// Configurar el puerto de forma predeterminada si no est� en la configuraci�n
if (!builder.Configuration.GetSection("Urls").Exists())
{
    builder.WebHost.UseUrls("http://localhost:5071");
}


// Configuraci�n de CORS - Simplificamos a una sola pol�tica
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Configuraci�n de SQLite
string dbPath = Path.Join(builder.Environment.ContentRootPath, "Data", "app.db");
Directory.CreateDirectory(Path.GetDirectoryName(dbPath));
builder.Services.AddDbContext<PasswordManagerContext>(options =>
    options.UseSqlite($"Data source={dbPath}")
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuraci�n del pipeline de solicitudes
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Inicializar la base de datos
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PasswordManagerContext>();
    context.Database.Migrate();
}

// Middleware en el orden correcto
app.UseStaticFiles(); // Sirve archivos desde wwwroot
app.UseRouting();
app.UseCors("AllowAll"); // Usa solo una pol�tica de CORS

app.MapControllers(); // Mapea los controladores de la API

// Configuraci�n del fallback para SPA
app.MapFallbackToFile("index.html"); // Asume que index.html est� en wwwroot

app.Run();