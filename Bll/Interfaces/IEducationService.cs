using Bll.Dto.Education;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Interfaces
{
    public interface IEducationService
    {
        Task<EducationReadDto> GetByIdAsync(int id);
        Task<IEnumerable<EducationReadDto>> GetByJobSeekerAsync(int jobSeekerId);
        Task<EducationReadDto> CreateAsync(EducationCreateDto dto);
        Task<EducationReadDto> UpdateAsync(int id, EducationUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
