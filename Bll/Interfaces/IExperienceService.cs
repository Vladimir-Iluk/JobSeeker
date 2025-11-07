using Bll.Dto.Experience;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Interfaces
{
    public interface IExperienceService
    {
        Task<ExperienceReadDto> GetByIdAsync(int id);
        Task<IEnumerable<ExperienceReadDto>> GetByJobSeekerAsync(int jobSeekerId);
        Task<ExperienceReadDto> CreateAsync(ExperienceCreateDto dto);
        Task<ExperienceReadDto> UpdateAsync(int id, ExperienceUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
