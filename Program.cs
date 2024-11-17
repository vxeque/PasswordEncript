//// a�adir el framework para la conexi�n a la db
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;
//using passwordManagent2.Models; // carpeta models donde se encuentra el modeloDB
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.EntityFrameworkCore.Sqlite;


//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//                      policy =>
//                      {
//                          // origen que permitiremos
//                          policy.WithOrigins("http://localhost:4321")
//                          .AllowAnyMethod()
//                          .AllowAnyHeader();

//                      });
//});


//// Add services to the container.
////builder.Services.AddDbContext<PasswordManagerContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//// Agrega la configuraci�n de SQLite

//// Configuraci�n de SQLite
//string dbPath = Path.Join(builder.Environment.ContentRootPath, "Data", "app.db");
//Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

//builder.Services.AddDbContext<PasswordManagerContext>(options =>
//    options.UseSqlite($"Data source={dbPath}"));



//// 
//builder.Services.AddControllers();

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


//// Inicializar la base de datos
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<PasswordManagerContext>();
//    context.Database.Migrate();
//}



////app.UseHttpsRedirection();


////app.UseEndpoints(endpoint => { endpoint.MapFallbackToFile("/")});

//app.UseRouting();

//app.UseStaticFiles();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapFallbackToFile("/frontend/index.html");
//    endpoints.MapControllers();
//});


//app.UseCors(MyAllowSpecificOrigins);

////app.UseAuthorization();

//app.MapControllers();

//app.Run();


//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using passwordManagent2.Models;
//using Microsoft.EntityFrameworkCore.Sqlite;

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
//var builder = WebApplication.CreateBuilder(args);

//// Configuraci�n de CORS
////builder.Services.AddCors(options =>
////{
////    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
////    {
////        policy.WithOrigins("http://localhost:4321")
////              .AllowAnyMethod()
////              .AllowAnyHeader();
////    });
////});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        builder =>
//        {
//            builder.AllowAnyOrigin()
//                   .AllowAnyMethod()
//                   .AllowAnyHeader();
//        });
//});


//// Configuraci�n de SQLite
//string dbPath = Path.Join(builder.Environment.ContentRootPath, "Data", "app.db");
//Directory.CreateDirectory(Path.GetDirectoryName(dbPath));

//builder.Services.AddDbContext<PasswordManagerContext>(options =>
//    options.UseSqlite($"Data source={dbPath}")
//);

//builder.Services.AddControllers();

//// Swagger
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configuraci�n del pipeline de solicitudes
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//// Inicializar la base de datos
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<PasswordManagerContext>();
//    context.Database.Migrate();
//}

//app.UseStaticFiles(); // Servir archivos est�ticos de wwwroot

//app.UseRouting(); // Debe ir antes de UseEndpoints

//app.UseCors(MyAllowSpecificOrigins); // Configuraci�n de CORS
//app.UseCors("AllowAll"); // Configuraci�n de CORS


////app.MapFallback(() => Results.File("./frontend/index.html"));
//app.MapFallback(() => Results.File("wwroot/frontend/index.html"));
////app.MapFallback(() => Results.File("D:\\Sources\\visual studio sources\\passwordManagent2\\passwordManagent2\\wwroot\\frontend\\index.html"));


////app.UseEndpoints(endpoints =>
////{
////    endpoints.MapControllers(); // Mapea los controladores de la API
////    endpoints.MapFallbackToFile("wwwroot/frontend/index.html"); // Fallback para el archivo HTML del frontend
////});

//app.Run();

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using passwordManagent2.Models;
using Microsoft.EntityFrameworkCore.Sqlite;

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