using Dal.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Interfaces
{
    public interface ICvRepository : IRepository<CvDto>
    {
        Task<IEnumerable<CvDto>> GetByJobSeekerIdAsync(int jobSeekerId, CancellationToken cancellationToken = default);
    }
}
