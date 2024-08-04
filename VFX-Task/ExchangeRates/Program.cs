using ExchangeRates.Interfaces;
using ExchangeRates.Repository;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// add serilog logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.File(".\\log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddLogging(x => x.AddSerilog()).BuildServiceProvider(true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add repositories
builder.Services.AddScoped<IDatabase, DataRepository>();
builder.Services.AddScoped<IExternalAPI, AlphaRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Finally, once just before the application exits...
Log.CloseAndFlush();