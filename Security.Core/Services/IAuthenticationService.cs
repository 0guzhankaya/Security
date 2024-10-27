using Security.Core.Dtos;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core.Services
{
	public interface IAuthenticationService
	{
		Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
		Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
		Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);
		Task<Response<ClientTokenDto>> CreateTokenByClient(ClientLoginDto clientLoginDto);
	}
}
