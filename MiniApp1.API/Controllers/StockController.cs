using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApp1.API.Controllers
{
	// Dummy Controller.
	[Authorize(Roles = "admin,manager")]
	[Route("api/[controller]")]
	[ApiController]
	public class StockController : ControllerBase
	{
		[Authorize(Policy = "AgePolicy")]
		[Authorize(Roles = "admin", Policy = "AnkaraPolicy")]
		[HttpGet]
		public IActionResult GetStock()
		{
			var userName = HttpContext.User.Identity.Name;
			var userIdClaim = User.Claims.FirstOrDefault(x  => x.Type == ClaimTypes.NameIdentifier);

			// veri tabanında userId veya userName alanları üzerinden gerekli dataları çek.

			// stockId stockQuantity Category UserId/UserName
			return Ok($"UserName: {userName} - UserId: {userIdClaim}");
		}
	}
}
