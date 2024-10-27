using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core.Dtos
{
	public class CreateUserDto
	{
		/// <summary>
		/// UserName bilgisi kullanıcıdan alınmayacağı zaman; email'inden @'e kadar olan kısım alınır
		/// ve veri çakışmasını önlemek için sonuna random birkaç sayı eklenebilir.
		/// Email : o-kaya@gmail.com --> UserName : o-kaya32421
		/// </summary>
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
