using Dal.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Interfaces
{
    public interface IEducationRepository : IRepository<EducationDto>
    {
        Task<IEnumerable<EducationDto>> GetByJobSeekerIdAsync(int jobSeekerId, CancellationToken cancellationToken = default);
    }
}
