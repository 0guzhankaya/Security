using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core.Configuration
{
	// Burada olan burada kalır, dışarı çıkmayacak.
	public class Client
	{
		public string Id { get; set; }
		public string Secret { get; set; }

		// Client, hangi API'lere erişebilir?
		// www.myapi1.com www.myapi2.com
		// Token'ın Payload kısmında gözükür.
		public List<string> Audiences { get; set; } 
	}
}
