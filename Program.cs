using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyFirstWebApp.Infrastructure;


// 1. CONFIG SERVICES -------------------------------------------------------------------
// Configure services: Controllers, Swagger, and others
var builder = WebApplication.CreateBuilder(args);

// Enables Controllers for handling HTTP requests
builder.Services.AddControllers();

// Enables API metadata generation for Swagger
builder.Services.AddEndpointsApiExplorer();

// Enables Swagger UI for visualizing and testing API endpoints
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


// 2. CONFIG DATABASE -------------------------------------------------------------------
// Set up SQLite in-memory database
var connection = new SqliteConnection("DataSource=:memory:");
connection.Open();

builder.Services.AddDbContext<TodoContext>(options =>
{
    options.UseSqlite(connection);
    // Enable sensitive logging only in development
    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine);
});


// 3. INITIALIZATION -------------------------------------------------------------------
// Build the app and initialize the database
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TodoContext>();
    context.Database.EnsureCreated();       // Ensure schema is created
    context.Seed();                         // Seed the database with test data
}


// 4. CONFIG MIDDLEWARE -------------------------------------------------------------------
// Middleware for handling requests and responses
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                       // Enable Swagger JSON
    app.UseSwaggerUI();                     // Enable Swagger UI
}

// app.UseHttpsRedirection();               // Redirect HTTP to HTTPS
// app.UseAuthentication();                 // Authenticate users
app.UseAuthorization();                     // Enforce authorization rules
app.MapControllers();                       // Map routes to controller actions


// 5. RUN THE APP -------------------------------------------------------------------
app.Run();
