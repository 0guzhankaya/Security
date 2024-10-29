using Microsoft.EntityFrameworkCore;
using Security.Core.Repositories;
using Security.Core.Services;
using Security.Core.UnitOfWork;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Security.Service.Services
{
	public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<TEntity> _genericRepository;

		public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> genericRepository)
		{
			_unitOfWork = unitOfWork;
			_genericRepository = genericRepository;
		}

		public async Task<Response<TDto>> AddAsync(TDto entityDto)
		{
			var entity = ObjectMapper.Mapper.Map<TEntity>(entityDto);
			await _genericRepository.AddAsync(entity);

			await _unitOfWork.CommitAsync();

			// Eklenen yeni datanın dto'sunun id'sinin doldurulması için.
			var dto = ObjectMapper.Mapper.Map<TDto>(entity);
			return Response<TDto>.Success(dto, 200);
		}

		public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
		{
			var products = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync());
			return Response<IEnumerable<TDto>>.Success(products, 200);
		}

		public async Task<Response<TDto>> GetByIdAsync(int id)
		{
			var product = await _genericRepository.GetByIdAsync(id);

			if (product == null)
				return Response<TDto>.Fail("Id not found!", 404, true);

			var entity = ObjectMapper.Mapper.Map<TDto>(product);
			return Response<TDto>.Success(entity, 200);
		}

		public async Task<Response<NoDataDto>> Remove(int id)
		{
			var isExistEntity = await _genericRepository.GetByIdAsync(id);

			if (isExistEntity == null)
				return Response<NoDataDto>.Fail("Id not found!", 404, true);

			// GetByIdAsync() ile gelen id'nin state'i Deleted'e çekildi.
			_genericRepository.Remove(isExistEntity);

			await _unitOfWork.CommitAsync();

			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<NoDataDto>> UpdateAsync(int id, TDto entityDto)
		{
			// Memory'de 2'si de takip edilirse hata verir. Bu yüzden Detached edildi.
			var isExistEntity = await _genericRepository.GetByIdAsync(id);

			if (isExistEntity == null)
				return Response<NoDataDto>.Fail("Id not found!", 404, true);

			var updateEntity = ObjectMapper.Mapper.Map<TEntity>(entityDto);

			// güncellenen entity burada yeniden takibe alınıyor.
			_genericRepository.Update(updateEntity);

			await _unitOfWork.CommitAsync();

			// update edilmiş data'yı dönmek request ile response akışını kalabalıklaştırır.
			return Response<NoDataDto>.Success(204);
		}

		public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
		{
			var list = _genericRepository.Where(predicate);

			// Pagination example
			// list.Skip(5).Take(10);

			var responseList = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync());

			return Response<IEnumerable<TDto>>.Success(responseList, 200);
		}
	}
}
