using Dal.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Interfaces
{
    public interface IExperienceRecordRepository : IRepository<ExperienceRecordDto>
    {
        Task<IEnumerable<ExperienceRecordDto>> GetByJobSeekerIdAsync(int jobSeekerId, CancellationToken cancellationToken = default);
    }

}
