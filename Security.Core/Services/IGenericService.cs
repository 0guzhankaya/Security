using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core.Services
{
	public interface IGenericService<TEntity, TDto> where TEntity : class where TDto : class
	{
		
		Task<Response<TDto>> GetByIdAsync(int id);
		Task<Response<IEnumerable<TDto>>> GetAllAsync();
		Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);
		Task<Response<TDto>> AddAsync(TDto entityDto);
		Task<Response<NoDataDto>> Remove(int id);
		Task<Response<NoDataDto>> UpdateAsync(int id, TDto entityDto);
	}
}
