using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApp1.API.Controllers
{
	// Dummy Controller.
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class InvoiceController : ControllerBase
	{
		[HttpGet]
		public IActionResult GetInvoice()
		{
			var userName = HttpContext.User.Identity.Name;
			var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

			// veri tabanında userId veya userName alanları üzerinden gerekli dataları çek.

			// stockId stockQuantity Category UserId/UserName
			return Ok($"Stock İşlemleri => UserName: {userName} - UserId: {userIdClaim}");
		}
	}
}
