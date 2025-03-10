using System.Globalization;
using System.Text;
using car_database_api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Load customer secrets based on the environment
byte[] key = null;
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
    key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSecret"] ?? throw new ArgumentNullException());
}
else if (builder.Environment.IsProduction())
{
    builder.Configuration.AddEnvironmentVariables();
    key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JwtSecret") ?? throw new ArgumentNullException());
}

// Register the DbContext with the connection string from customer secrets
var connectionString = builder.Environment.IsDevelopment() ?
    builder.Configuration.GetConnectionString("DevelopmentConnection")  + ";TrustServerCertificate=True":
    builder.Configuration.GetConnectionString("DeploymentConnection") + ";TrustServerCertificate=True";

builder.Services.AddDbContext<CarRentalDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));

// Add controllers
builder.Services.AddControllers();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Register HttpClient
builder.Services.AddHttpClient();

// Configure JWT Authentication
var logger = LoggerFactory.Create(b => b.AddConsole()).CreateLogger<Program>();

// var jwtSecret = builder.Configuration["Jwt__Secret"];
// if (string.IsNullOrEmpty(jwtSecret))
// {
//     logger.LogError("JWT secret is not configured.");
//     throw new InvalidOperationException("JWT secret is not configured.");
// }
// var key = Encoding.ASCII.GetBytes(jwtSecret);
// var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt__Secret"] ?? throw new ArgumentNullException());
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Car Rental API", Version = "v1" });
    
    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
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
    
// Build the service provider
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Apply the CORS policy
app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();
