using Bll.Dto.Cv;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Interfaces
{
    public interface ICvService
    {
        Task<CvReadDto> GetByIdAsync(int id);
        Task<IEnumerable<CvReadDto>> GetByJobSeekerAsync(int jobSeekerId);
        Task<CvReadDto> CreateAsync(CvCreateDto dto);
        Task<CvReadDto> UpdateAsync(int id, CvUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
