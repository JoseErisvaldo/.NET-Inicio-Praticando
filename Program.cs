using Microsoft.EntityFrameworkCore;
using MinhaApi.Data;
using AutoMapper;
using MinhaApi.Services;
using MinhaApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// 🌐 1) Definir porta para a Railway
// ---------------------------------------------------------
builder.WebHost.ConfigureKestrel(options =>
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
    options.ListenAnyIP(int.Parse(port));
});

// ---------------------------------------------------------
// 🔌 2) Obter connection string (local → Railway)
// ---------------------------------------------------------
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Se não existir no appsettings.json, monta usando variáveis da Railway
if (string.IsNullOrWhiteSpace(connectionString))
{
    var host = Environment.GetEnvironmentVariable("MYSQLHOST") ?? "localhost";
    var port = Environment.GetEnvironmentVariable("MYSQLPORT") ?? "3306";
    var user = Environment.GetEnvironmentVariable("MYSQLUSER") ?? "appuser";
    var pwd = Environment.GetEnvironmentVariable("MYSQLPASSWORD") ?? "appuser123";
    var db = Environment.GetEnvironmentVariable("MYSQLDATABASE") ?? "produtos_db";

    connectionString =
        $"Server={host};Port={port};Database={db};Uid={user};Pwd={pwd};AllowPublicKeyRetrieval=True;SslMode=None";
}

// Registrar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


// ---------------------------------------------------------
// 🧱 3) Serviços, Repo, AutoMapper, etc
// ---------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();


// ---------------------------------------------------------
// 🗄️ 4) Aplicar migrations automaticamente
// ---------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        db.Database.Migrate();
        Console.WriteLine("✔ Migrations applied successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Migration error: " + ex.Message);
    }
}


// ---------------------------------------------------------
// 🚀 5) Pipeline
// ---------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
