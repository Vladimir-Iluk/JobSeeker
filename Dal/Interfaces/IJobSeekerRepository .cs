using Dal.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Interfaces
{
    public interface IJobSeekerRepository : IRepository<JobSeekerDto>
    {
        Task<IEnumerable<JobSeekerDto>> SearchByNameOrSkillAsync(string query, CancellationToken cancellationToken = default);
    }
}
