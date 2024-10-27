using Security.Core.Configuration;
using Security.Core.Dtos;
using Security.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core.Services
{
	public interface ITokenService
	{
		TokenDto CreateToken(UserApp userApp);
		ClientTokenDto CreateTokenByClient(Client client);
	}
}
