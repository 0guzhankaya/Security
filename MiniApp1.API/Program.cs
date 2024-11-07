using Shared.Configuration;
using System.Configuration;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Custom Token configuration
builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));

// TokenOptions'u doðrudan Configuration üzerinden alarak Auth iþlemi eklenir
builder.Services.AddCustomTokenAuth(builder);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
