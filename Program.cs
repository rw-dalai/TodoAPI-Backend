using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MyFirstWebApp.Infrastructure;

// Add this into MyFirstWebApp.csproj in the <PropertyGroup>..</PropertyGroup>

// <PropertyGroup>
//   ...
//   <GenerateDocumentationFile>true</GenerateDocumentationFile>
//   <NoWarn>$(NoWarn);1591</NoWarn> <!-- Suppress warnings for missing comments -->
// </PropertyGroup>


// 1. CONFIG SERVICES -------------------------------------------------------------------
// Set up the app and configure services

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();              // Adds controllers
builder.Services.AddEndpointsApiExplorer();     // Adds API explorer for OpenAPI
builder.Services.AddSwaggerGen(options =>       // Adds Swagger
{    
    var xmlFilename = $"{System.Reflection.Assembly
        .GetExecutingAssembly().GetName().Name}.xml";
    
    options.IncludeXmlComments(
        Path.Combine(AppContext.BaseDirectory, xmlFilename));
});



// 2. CONFIG DATABASE -------------------------------------------------------------------
// Set up SQLite in-memory database and open a connection

var connection = new SqliteConnection("DataSource=:memory:");
connection.Open();

builder.Services.AddDbContext<TodoContext>(options => {
    options.UseSqlite(connection);              // Use SQLite as the Database Provider
    if (builder.Environment.IsDevelopment())    // Are we in Development mode?
        options.EnableSensitiveDataLogging()    // -> Enable sensitive data logging
            .LogTo(Console.WriteLine);
});



// 3. INITIALIZATION APP ----------------------------------------------------------------
// Build the app and initialize the database

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var context = scope.ServiceProvider         // Get the TodoContext  
        .GetRequiredService<TodoContext>();

    context.Database.EnsureCreated();           // Ensure the database is created
    if (app.Environment.IsDevelopment())        // Are we in Development mode?
        context.Seed();                         // -> Seed the database
}



// 4. SETUP MIDDLEWARE ------------------------------------------------------------------
// Middleware processes HTTP requests/responses in a pipeline doing authentication, logging, or routing.
// Middleware is executed in the order it is added to the pipeline.

if (app.Environment.IsDevelopment()) {          // Are we in Development mode?
    app.UseSwagger();                           // -> Enable Swagger JSON
    app.UseSwaggerUI();                         // -> Enable Swagger UI
}

// Optional middleware
// app.UseHttpsRedirection();                   // Redirect HTTP to HTTPS
// app.UseStaticFiles();                        // Serve static files like CSS or JS
app.UseRouting();                               // Enable endpoint routing
// app.UseAuthentication();                     // Authenticate the user
// app.UseAuthorization();                      // Check user permissions
app.MapControllers();                           // Map controller routes (last step)



// 5. RUN THE APP -----------------------------------------------------------------------
app.Run();
