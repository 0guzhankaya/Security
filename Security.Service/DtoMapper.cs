using AutoMapper;
using Security.Core.Dtos;
using Security.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Service
{
	internal class DtoMapper : Profile
	{
		public DtoMapper() 
		{
			CreateMap<ProductDto, Product>().ReverseMap();
			CreateMap<UserAppDto, UserApp>().ReverseMap();
		}
	}
}
