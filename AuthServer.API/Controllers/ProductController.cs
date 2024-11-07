﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Core.Dtos;
using Security.Core.Models;
using Security.Core.Services;

namespace AuthServer.API.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : CustomBaseController
	{
		private readonly IGenericService<Product, ProductDto> _productService;

		public ProductController(IGenericService<Product, ProductDto> productService)
		{
			_productService = productService;
		}

		[HttpGet]
		public async Task<IActionResult> GetProduct()
		{
			return ActionResultInstance(await _productService.GetAllAsync());
		}

		[HttpPost]
		public async Task<IActionResult> SaveProduct(ProductDto productDto)
		{
			return ActionResultInstance(await _productService.AddAsync(productDto));
		}

		[HttpPut]
		public async Task<IActionResult> UpdateProduct(ProductDto productDto)
		{
			return ActionResultInstance(await _productService.UpdateAsync(productDto.Id, productDto));
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			return ActionResultInstance(await _productService.Remove(id));
		}
	}
}