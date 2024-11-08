using Microsoft.AspNetCore.Authorization;

namespace MiniApp1.API.Requirements
{
	public class BirthDayRequirement : IAuthorizationRequirement
	{
		// policy based auth gerçekleştirmek için IAuthorizationRequirement'den inheritance alınır.

		public int Age { get; set; }

		public BirthDayRequirement(int age)
		{
			Age = age;
		}
	}

	public class BirthDayRequirementHandler : AuthorizationHandler<BirthDayRequirement>
	{
		// business kodlarının yazılacağı class.
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BirthDayRequirement requirement)
		{
			var birthDate = context.User.FindFirst("birth-date");

			if (birthDate == null)
			{
				context.Fail();
				return Task.CompletedTask; // İşlemi bitirir.
			}

			var today = DateTime.Now;

			var age = today.Year - Convert.ToDateTime(birthDate.Value).Year;

			// 23 >= 18
			if (age >= requirement.Age)
			{
				context.Succeed(requirement);
			}
			else 
				context.Fail();

			return Task.CompletedTask;
		}
	}
}
