using SM.Application;
using SM.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapAuthenticationEndpoints();
app.MapCommentEndpoints();
app.MapPostEndpoints();
app.MapReactionEndpoints();
app.MapUserEndpoints();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.Run();
