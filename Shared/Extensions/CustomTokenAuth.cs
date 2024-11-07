using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Configuration;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Extensions
{
	public static class CustomTokenAuth
	{
		public static void AddCustomTokenAuth(this IServiceCollection services, WebApplicationBuilder builder)
		{
			var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidIssuer = tokenOptions.Issuer,
					ValidAudience = tokenOptions.Audience[0],
					IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

					ValidateIssuerSigningKey = true,
					ValidateAudience = true,
					ValidateIssuer = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero, // Server'daki saat farklarını tolare etmek için eklenen zamanı sıfırlar. Default 5 dakika. Opsiyonel.
				};
			});
		}
	}
}
