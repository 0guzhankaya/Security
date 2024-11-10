using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
		private readonly IMemoryCache _memoryCache;

		public ProductController(IGenericService<Product, ProductDto> productService, IMemoryCache memoryCache)
		{
			_productService = productService;
			_memoryCache = memoryCache;
		}

		[HttpGet]
		public async Task<IActionResult> GetProduct()
		{
			// time key'ine sahip cachelenecek data'yı almak ister,
			// data eğer yoksa oluşturur.
			_memoryCache.GetOrCreate<string>("time", entry =>
			{
				return DateTime.Now.ToString();
			});

			_memoryCache.TryGetValue("callback", out var callback);
			var callbackCache = callback; // gösterilmesi için.

			// var cache = _memoryCache.Get<string>("time");
			return ActionResultInstance(await _productService.GetAllAsync());
		}

		[HttpPost]
		public async Task<IActionResult> SaveProduct(ProductDto productDto)
		{
			// 1.Yol
			if (String.IsNullOrEmpty(_memoryCache.Get<string>("time")))
			{
				_memoryCache.Set<string>("time", DateTime.Now.ToString());
			}

			// 2.Yol : Önerilir.
			// time cache'ini alabilirse geri hem true döner hem de timeCache'e time key'ine sahip değeri cache'i atar.
			// eğer true dönerse, - cache varsa - timeCache ile ilgili cache değeri alınıp kullanılabilir.
			if (!_memoryCache.TryGetValue("time", out var timeCache))
			{
				MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
				options.SlidingExpiration = TimeSpan.FromSeconds(10);
				options.AbsoluteExpiration = DateTime.Now.AddSeconds(30);
				options.Priority = CacheItemPriority.Normal;
				options.RegisterPostEvictionCallback((key, value, reason, state) =>
				{
					_memoryCache.Set("callback", $"{key}->{value} => reason:{reason}");
				});

				_memoryCache.Set<string>("time", DateTime.Now.ToString(), options);
			}

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
			_memoryCache.Remove("time");
			return ActionResultInstance(await _productService.Remove(id));
		}
	}
}
