using Microsoft.Azure.Cosmos;
using Models.Models;
using AutoMapper;
using Core.Service;
using System.Security.Policy;
using Core.Data;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<FakeDataGenerator>();

string instrumentationKey = builder.Configuration["ApplicationInsights:InstrumentationKey"];
string ingestionEndpoint = builder.Configuration["ApplicationInsights:IngestionEndpoint"];

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
//    .Enrich.FromLogContext()
//    .WriteTo.ApplicationInsights(
//        $"InstrumentationKey={instrumentationKey};IngestionEndpoint={ingestionEndpoint}",
//        TelemetryConverter.Traces)
//    .CreateLogger();


string Url = builder.Configuration.GetSection("AzureCosmosDBSettings")
    .GetValue<string>("URL");
string primaryKey = builder.Configuration.GetSection("AzureCosmosDBSettings")
    .GetValue<string>("PrimaryKey");

builder.Services.AddSingleton<CosmosClient>(provider => new CosmosClient(Url, primaryKey));

builder.Services.AddScoped<IAccountRepository>(options =>
{
    string databaseName = builder.Configuration.GetSection("AzureCosmosDBSettings")
        .GetValue<string>("DatabaseName");

    var cosmosClient = options.GetService<CosmosClient>(); // Inject the CosmosClient service
    var fakeDataGenerator = options.GetService<FakeDataGenerator>(); // Inject the FakeDataGenerator service

    return new AccountRepository(cosmosClient, databaseName, fakeDataGenerator);
});



builder.Services.AddScoped<IMessageRepository>(options =>
{
    string databaseName = builder.Configuration.GetSection("AzureCosmosDBSettings")
    .GetValue<string>("DatabaseName");
    //string containerName = builder.Configuration.GetSection("AzureCosmosDBSettings")
    //.GetValue<string>("ContainerName");

    var cosmosClient = new CosmosClient(
        Url,
        primaryKey
    );

    return new MessageRepository(cosmosClient, databaseName);
});

builder.Services.AddScoped<IReactionRepository>(options =>
{
    string databaseName = builder.Configuration.GetSection("AzureCosmosDBSettings")
    .GetValue<string>("DatabaseName");
    //string containerName = builder.Configuration.GetSection("AzureCosmosDBSettings")
    //.GetValue<string>("ContainerName");

    var cosmosClient = new CosmosClient(
        Url,
        primaryKey
        );

    return new ReactionRepository(cosmosClient, databaseName);
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
