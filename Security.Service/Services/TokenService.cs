using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Security.Core.Configuration;
using Security.Core.Dtos;
using Security.Core.Models;
using Security.Core.Services;
using Shared.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Shared.Services;

namespace Security.Service.Services
{
	public class TokenService : ITokenService
	{
		// UserManager'dan nesne örneği için DI
		private readonly UserManager<UserApp> _userManager;
		private readonly CustomTokenOptions _customTokenOptions;

		public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOptions> customTokenOptions)
		{
			_userManager = userManager;
			_customTokenOptions = customTokenOptions.Value;
		}

		// Guid'den daha unique bir random byte oluşturucu.
		private string CreateRefreshToken()
		{
			var numberByte = new byte[32];

			using var random = RandomNumberGenerator.Create();

			random.GetBytes(numberByte);

			return Convert.ToBase64String(numberByte);
		}

		private IEnumerable<Claim> GetClaims(UserApp userApp, List<String> audiences)
		{
			var userList = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, userApp.Id), 
				new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
				new Claim(ClaimTypes.Name, userApp.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

			return userList;
		}

		private IEnumerable<Claim> GetClaimsByClient(Client client)
		{
			var claims = new List<Claim>();
			claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
			new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString());

			return claims;
		}

		public TokenDto CreateToken(UserApp userApp)
		{
			var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccessTokenExpiration);
			var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.RefreshTokenExpiration);
			var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptions.SecurityKey);
			SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

			JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
				issuer: _customTokenOptions.Issuer,
				expires: accessTokenExpiration,
				notBefore: DateTime.Now,
				claims: GetClaims(userApp, _customTokenOptions.Audience),
				signingCredentials: signingCredentials
				);

			var handler = new JwtSecurityTokenHandler();
			var token = handler.WriteToken(jwtSecurityToken);

			var tokenDto = new TokenDto
			{
				AccessToken = token,
				RefreshToken = CreateRefreshToken(),
				AccessTokenExpression = accessTokenExpiration,
				RefreshTokenExpiration = refreshTokenExpiration,
			};

			return tokenDto;
		}

		public ClientTokenDto CreateTokenByClient(Client client)
		{
			var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccessTokenExpiration);
			var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOptions.SecurityKey);

			SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

			JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
				issuer: _customTokenOptions.Issuer,
				expires: accessTokenExpiration,
				notBefore: DateTime.Now,
				claims: GetClaimsByClient(client),
				signingCredentials: signingCredentials);

			var handler = new JwtSecurityTokenHandler();
			var token = handler.WriteToken(jwtSecurityToken);

			var tokenDto = new ClientTokenDto
			{
				AccessToken = token,
				AccessTokenExpiration = accessTokenExpiration,
			};

			return tokenDto;
		}
	}
}
