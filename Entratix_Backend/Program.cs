using DataAccess;
using IDataAccess;
using DataAccess.Contexts;
using IBusinessLogic;
using BusinessLogic;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Load appsettings.json configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Imprime la cadena de conexión en la consola para verificar que esté siendo leída correctamente
var connectionString = config.GetConnectionString("PostgreSQL");
Console.WriteLine($"Cadena de conexión a la base de datos: {connectionString}");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserLogic, UserLogic>();

// Add DbContexto to the service container
builder.Services.AddDbContext<DbContexto>(options =>
    options.UseNpgsql(connectionString));

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy =>
        {
            policy.AllowAnyOrigin()
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

// Use CORS
app.UseCors("CorsPolicy");


//Imprimir todos los usuarios sin hacer un endpoint
using (var scope = app.Services.CreateScope())
{
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var users = userService.GetUsers();
    foreach (var user in users)
    {
        Console.WriteLine(user);
    }
}

app.UseAuthorization();

app.MapControllers();

app.Run();
