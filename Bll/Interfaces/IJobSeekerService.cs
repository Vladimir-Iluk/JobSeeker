using Bll.Dto.JobSeeker;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Interfaces
{
    public interface IJobSeekerService
    {
        Task<JobSeekerReadDto> GetByIdAsync(int id);
        Task<IEnumerable<JobSeekerReadDto>> GetAllAsync();
        Task<JobSeekerReadDto> CreateAsync(JobSeekerCreateDto dto);
        Task<JobSeekerReadDto> UpdateAsync(int id, JobSeekerUpdateDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<JobSeekerReadDto>> SearchAsync(string query);
    }
}
