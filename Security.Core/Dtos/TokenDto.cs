﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core.Dtos
{
	public class TokenDto
	{
		public string AccessToken { get; set; }
		public DateTime AccessTokenExpression { get; set; }
		public string RefreshToken { get; set; }
		public DateTime RefreshTokenExpiration { get; set; }
	}
}