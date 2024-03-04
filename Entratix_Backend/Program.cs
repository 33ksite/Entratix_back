using DataAccess;
using IDataAccess;
using DataAccess.Contexts;
using IBusinessLogic;
using BusinessLogic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Load appsettings.json configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = config.GetConnectionString("PostgreSQL");
Console.WriteLine($"Cadena de conexión a la base de datos: {connectionString}");
builder.Services.AddDbContext<DbContexto>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserLogic, UserLogic>();
builder.Services.AddScoped<IEventService, EventService>();


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

app.UseAuthorization();

app.MapControllers();

app.Run();
