﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core.Dtos
{
	public class ClientLoginDto
	{
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
	}
}
