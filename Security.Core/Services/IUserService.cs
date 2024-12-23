﻿using Microsoft.AspNetCore.Http.HttpResults;
using Security.Core.Dtos;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core.Services
{
	public interface IUserService
	{
		Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
		Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
		Task<Response<NoContent>> CreateUserRoles(string userName);
	}
}
