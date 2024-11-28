using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyFirstWebApp.Infrastructure;


// 1. CONFIG SERVICES -------------------------------------------------------------------
// Set up the app and configure services
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();              // Adds controllers to the app
builder.Services.AddEndpointsApiExplorer();     // Adds API explorer for generating OpenAPI documentation
builder.Services.AddSwaggerGen(options =>       // Adds Swagger to the app
{
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});



// 2. CONFIG DATABASE -------------------------------------------------------------------
// Set up SQLite in-memory database and open a connection
var connection = new SqliteConnection("DataSource=:memory:");
connection.Open();

builder.Services.AddDbContext<TodoContext>(options =>
{
    options.UseSqlite(connection);              // Use SQLite as the database provider
    if (builder.Environment.IsDevelopment())    // If the app is running in development mode
        options.EnableSensitiveDataLogging()    // -> Enable sensitive data logging
            .LogTo(Console.WriteLine);
});



// 3. INITIALIZATION APP ----------------------------------------------------------------
// Build the app and initialize the database
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider         // Get the TodoContext from the service provider  
        .GetRequiredService<TodoContext>();

    context.Database.EnsureCreated();           // Ensure the database is created
    if (app.Environment.IsDevelopment())        // If the app is running in development mode
        context.Seed();                         // -> Seed the database
}



// 4. SETUP MIDDLEWARE ------------------------------------------------------------------
// Middleware for handling requests and responses
if (app.Environment.IsDevelopment())            // If the app is running in development mode
{
    app.UseSwagger();                           // Enable Swagger JSON
    app.UseSwaggerUI();                         // Enable Swagger UI
}

// app.UseHttpsRedirection();                   // Redirect HTTP to HTTPS
// app.UseAuthentication();                     // Authenticate users
// app.UseAuthorization();                      // Authorize users
app.MapControllers();                           // Map routes to controller actions



// 5. RUN THE APP -----------------------------------------------------------------------
app.Run();
