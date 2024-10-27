using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Dtos
{
	public class Response<T> where T : class
	{
		public T Data { get; private set; }
		public int StatusCode { get; private set; }

		// Business logic'ini azaltmak için, sadece backend'de kullanılacak.
		[JsonIgnore]
		public bool IsSuccessfull { get; private set; }
		public ErrorDto Error { get; private set; }

		public static Response<T> Success(T data, int statusCode)
		{
			return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessfull = true };
		}

		// Güncelleme ve silme işlemlerinde geriye data dönmeye gerek yoktur.
		public static Response<T> Success(int statusCode)
		{
			return new Response<T> { Data = default, StatusCode = statusCode, IsSuccessfull = true };
		}

		public static Response<T> Fail(ErrorDto errorDto, int statusCode)
		{
			return new Response<T> { Error = errorDto, StatusCode = statusCode };
		}

		public static Response<T> Fail(string errorMessage, int statusCode, bool isShow)
		{
			var errorDto = new ErrorDto(errorMessage, isShow);

			return new Response<T> { Error = errorDto, StatusCode=statusCode };
		} 
		
	}
}
