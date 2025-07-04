using EntityFrameworkCore.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Configure JSON serialization options to ignore cyclic references
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; //cip...104
}); //cip...104
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var sqliteDatabaseName = builder.Configuration.GetConnectionString("SqliteDatabaseConnectionString"); //cip...99
//-----------------------------------------------------------
// copied from FootballLeagueDbContext.cs constructor
var folder = Environment.SpecialFolder.LocalApplicationData;
var path = Environment.GetFolderPath(folder);
var dbPath = Path.Combine(path, sqliteDatabaseName);
//-----------------------------------------------------------
//var connectionString = $"Data Source={dbPath};"; //cip...99
var connectionString = builder.Configuration.GetConnectionString("SqlServerDatabaseConnectionString"); //cip...99

builder.Services.AddDbContext<FootballLeagueDbContext>(options =>
{
    //options allows replacement of the FootballLeagueDbContext.OnConfiguring method //cip...99
    options.UseSqlServer(connectionString, sqlserverOptions =>
    {
        //sqlserverOptions.MigrationsAssembly("EntityFrameworkCore.Data");
        //sqlserverOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        //sqlserverOptions.MigrationsHistoryTable("__EFMigrationsHistory", "dbo"); //tw mention
        //sqlserverOptions.CommandTimeout(30); //cip...115. Set command timeout to 30 seconds
        sqlserverOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null); //cip...115 Enable retry on failure for transient errors
    }) //cip...99
        //options.UseSqlite(connectionString) //cip...99
        //.UseLazyLoadingProxies()
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        .LogTo(Console.WriteLine, LogLevel.Information);

    if (!builder.Environment.IsProduction()) // Only enable sensitive data logging and detailed errors when NOT in Production
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});


// Add support for discovering minimal API endpoints (used by Swagger)
builder.Services.AddEndpointsApiExplorer();
// Register the Swagger generator service (used to generate OpenAPI docs)
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    // Enable middleware to serve generated Swagger JSON
    app.UseSwagger();
    // Enable the Swagger UI middleware to serve the Swagger UI at /swagger
    app.UseSwaggerUI();
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
