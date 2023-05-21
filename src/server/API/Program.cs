using Microsoft.Azure.Cosmos;
using Models.Models;
using AutoMapper;
using Core.Service.Reaction;
using Core.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IReaction<Account>>(options =>
{
    string Url = builder.Configuration.GetSection("AzureCosmosDBSettings")
    .GetValue<string>("URL");
    string primaryKey = builder.Configuration.GetSection("AzureCosmosDBSettings")
    .GetValue<string>("PrimaryKey");
    string databaseName = builder.Configuration.GetSection("AzureCosmosDBSettings")
    .GetValue<string>("DatabaseName");
    //string containerName = builder.Configuration.GetSection("AzureCosmosDBSettings")
    //.GetValue<string>("ContainerName");

    var cosmosClient = new CosmosClient(
        Url,
        primaryKey
        );

    return new AccountRepository(cosmosClient, databaseName);
});

builder.Services.AddScoped<IReaction<Message>>(options =>
{
    string Url = builder.Configuration.GetSection("AzureCosmosDBSettings")
    .GetValue<string>("URL");
    string primaryKey = builder.Configuration.GetSection("AzureCosmosDBSettings")
    .GetValue<string>("PrimaryKey");
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
    string Url = builder.Configuration.GetSection("AzureCosmosDBSettings")
    .GetValue<string>("URL");
    string primaryKey = builder.Configuration.GetSection("AzureCosmosDBSettings")
    .GetValue<string>("PrimaryKey");
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
