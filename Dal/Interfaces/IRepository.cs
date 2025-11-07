using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Interfaces
{
    public interface IRepository<TDto> where TDto : class
    {
        Task<TDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<int> CreateAsync(TDto dto, CancellationToken cancellationToken = default);
        Task UpdateAsync(TDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
