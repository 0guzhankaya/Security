using Shared.Configuration;
using System.Configuration;
using Shared.Extensions;
using MiniApp1.API.Requirements;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Custom Token configuration
builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();

// TokenOptions'u doðrudan Configuration üzerinden alarak Auth iþlemi eklenir
builder.Services.AddCustomTokenAuth(builder);

// Handler is defined.
builder.Services.AddSingleton<IAuthorizationHandler, BirthDayRequirementHandler>();

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AnkaraPolicy", policy =>
	{
		policy.RequireClaim("city", "Ankara");
	});
	options.AddPolicy("AgePolicy", policy =>
	{
		policy.Requirements.Add(new BirthDayRequirement(18));
	});
});

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
