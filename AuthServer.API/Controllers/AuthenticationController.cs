﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Security.Core.Dtos;
using Security.Core.Services;

namespace AuthServer.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AuthenticationController : CustomBaseController
	{
		private readonly IAuthenticationService _authenticationService;

		public AuthenticationController(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
		}

		// api/authentication/createtoken
		[HttpPost]
		public async Task<IActionResult> CreateToken(LoginDto loginDto)
		{
			var result = await _authenticationService.CreateTokenAsync(loginDto);

			return ActionResultInstance(result);
		}

		[HttpPost]
		public IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
		{
			var result = _authenticationService.CreateTokenByClient(clientLoginDto);
			return ActionResultInstance(result);
		}

		[HttpPost]
		public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
		{
			var result = await _authenticationService.RevokeRefreshToken(refreshTokenDto.RefreshToken);
			return ActionResultInstance(result);
		}

		[HttpPost]
		public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
		{
			var result = await _authenticationService.CreateTokenByRefreshToken(refreshTokenDto.RefreshToken);
			return ActionResultInstance(result);
		}
	}
}
