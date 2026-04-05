using backend_gym_webapp.Data;
using backend_gym_webapp.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ==================== SERVICE CONFIGURATION ====================
// This section adds all the services our application needs

// 1. Add Entity Framework Core with SQL Server
// DefaultConnection - tells EF where the database is located
// Update the connection string with your actual SQL Server details
builder.Services.AddDbContext<GymDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Server=.;Database=GymManagementDb;Trusted_Connection=True;"));

// 2. Add our custom services (Dependency Injection)
// This registers GymClassService so it can be used throughout the app
builder.Services.AddScoped<IGymClassService, GymClassService>();

// 3. Add OpenAPI/Swagger documentation
builder.Services.AddOpenApi();

// 4. Add Swagger documentation
builder.Services.AddSwaggerGen();

// 5. Add Controllers support
builder.Services.AddControllers();

// 6. Add Scalar UI for beautiful API documentation (extension already provided by Scalar.AspNetCore package)
// No need to call AddScalarUI() - it's built into the package

// ==================== BUILD THE APP ====================
var app = builder.Build();

// ==================== CONFIGURE PIPELINE ====================
// Configure the HTTP request pipeline

if (app.Environment.IsDevelopment())
{
    // Enable OpenAPI endpoint for API documentation
    app.MapOpenApi();

    // Enable Swagger UI - interactive API documentation
    // Available at: https://localhost:7130/swagger/index.html
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gym Management API v1");
        options.RoutePrefix = "swagger"; // Access at /swagger
    });
}

// Enable HTTPS redirection for security
app.UseHttpsRedirection();

// Map all controller routes (our API endpoints)
app.MapControllers();
app.UseRouting();
// ==================== RUN THE APP ====================
app.Run();

//==================== DATABASE MIGRATION ====================
/*
In case you need to update the database schema, uncomment and run the following lines once:
dotnet ef migrations add InitialCreate
dotnet ef database update
*/

// Microsoft.Hosting.Lifetime: Information: Now listening on: https://localhost:7130
// Microsoft.Hosting.Lifetime: Information: Now listening on: http://localhost:5170
// Microsoft.Hosting.Lifetime: Information: Application started. Press Ctrl+C to shut down.
