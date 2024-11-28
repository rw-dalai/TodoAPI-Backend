var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

// Configure Swagger to include XML comments
builder.Services.AddSwaggerGen(options =>
{
    // Dynamically generate the path for the XML documentation file
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";

    // Include XML comments in Swagger
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(); // Generate Swagger JSON
    app.UseSwaggerUI(); // Enable Swagger UI
}


app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();