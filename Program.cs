using backend_gym_webapp.Data;
using backend_gym_webapp.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ==================== SERVICE CONFIGURATION ====================
// This section adds all the services our application needs

// 1. Add Entity Framework Core with SQL Server
// This connects our app with the database
builder.Services.AddDbContext<GymDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Server=.;Database=GymManagementDb;Trusted_Connection=True;"));

// 2. Add GymClass service
builder.Services.AddScoped<IGymClassService, GymClassService>();

// 3. Add Trainer service
builder.Services.AddScoped<TrainerService>();

// 4. Add MembershipPlan service
builder.Services.AddScoped<MembershipPlanService>();

// 5. Add Dashboard service
builder.Services.AddScoped<DashboardService>();

// 6. Add OpenAPI
builder.Services.AddOpenApi();

// 7. Add Swagger
builder.Services.AddSwaggerGen();

// 8. Add Controllers
builder.Services.AddControllers();

// ==================== BUILD THE APP ====================
var app = builder.Build();

// ==================== CONFIGURE PIPELINE ====================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gym Management API v1");
        options.RoutePrefix = "swagger";
    });
}

// Enable HTTPS
app.UseHttpsRedirection();

// Enable routing
app.UseRouting();

// Map controllers (API endpoints)
app.MapControllers();

// ==================== RUN THE APP ====================
app.Run();