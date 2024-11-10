using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Security.Core.Configuration;
using Security.Core.Models;
using Security.Core.Repositories;
using Security.Core.Services;
using Security.Core.UnitOfWork;
using Security.DataAccess;
using Security.DataAccess.Repositories;
using Security.Service.Services;
using Shared.Configuration;
using Shared.Extensions;
using Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// DI Register
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddIdentity<UserApp, IdentityRole>(options =>
{
	options.User.RequireUniqueEmail = true;
	options.Password.RequireNonAlphanumeric = false; // production'da true.
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), sqlOptions =>
	{
		sqlOptions.MigrationsAssembly("Security.DataAccess");
	});
});

// Custom Token configuration.
builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));

// Token doðrulamasý.
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
	var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();
	options.TokenValidationParameters = new TokenValidationParameters()
	{
		ValidIssuer = tokenOptions.Issuer,
		ValidAudience = tokenOptions.Audience[0],
		IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

		ValidateIssuerSigningKey = true,
		ValidateAudience = true,
		ValidateIssuer = true,
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero, // Server'daki saat farklarýný tolare etmek için eklenen zamaný sýfýrlar. Default 5 dakika. Opsiyonel.
	};
});

// Add services to the container.
builder.Services.AddControllers().AddFluentValidation(options =>
{
	options.RegisterValidatorsFromAssemblyContaining<Program>();
});

// Custom validation comes from shared layer.
builder.Services.UseCustomValidationResponse();

// In-Memory Cache
builder.Services.AddMemoryCache();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Custom Exception Middleware which have Run() method.
app.UseCustomException();

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
