// Add services to the container.
using DataAccess;
using IDataAccess;
using DataAccess.Contexts;
using IBusinessLogic;
using BusinessLogic;
using Microsoft.EntityFrameworkCore;
using Entratix_Backend;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Entratix_Backend.Utilities;

var builder = WebApplication.CreateBuilder(args);
var myOrigins = "_myOrigins";
ConfigurationManager configuration = builder.Configuration;

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myOrigins,
            policy =>
            {
                policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(hostName => true);
            });
});
// Load appsettings.json configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("ApplicationSettings"));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie(x => x.Cookie.Name = "token").AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["ApplicationSettings:Secret"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["X-Access-Token"];
            return Task.CompletedTask;
        }
    };
});

var connectionString = configuration.GetConnectionString("PostgreSQL");
Console.WriteLine($"Cadena de conexion a la base de datos: {connectionString}");
builder.Services.AddDbContext<DbContexto>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Entratix_Backend")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthLogic, AuthLogic>();
builder.Services.AddScoped<IUserLogic, UserLogic>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ITicketPurchaseService, TicketPurchaseService>();
builder.Services.AddScoped<ITicketPurchaseLogic, TicketPurchaseLogic>();
builder.Services.AddSingleton<TokenManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(myOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
