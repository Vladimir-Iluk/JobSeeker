using AutoMapper;
using FluentValidation;
using Bll.Dto.Education;
using Bll.Exceptions;
using Bll.Interfaces;
using Bll.Validators.Education;
using Dal.Dto;
using Dal.Interfaces;

namespace Bll.Services
{
    public class EducationService : IEducationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly EducationValidator _createValidator;
        private readonly EducationUpdateValidator _updateValidator;

        public EducationService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
            _createValidator = new EducationValidator();
            _updateValidator = new EducationUpdateValidator();
        }

        public async Task<EducationReadDto> GetByIdAsync(int id)
        {
            var entity = await _uow.Educations.GetByIdAsync(id)
                ?? throw new NotFoundException($"Education {id} not found");

            return _mapper.Map<EducationReadDto>(entity);
        }

        public async Task<IEnumerable<EducationReadDto>> GetByJobSeekerAsync(int jobSeekerId)
        {
            var items = await _uow.Educations.GetByJobSeekerIdAsync(jobSeekerId);
            return _mapper.Map<IEnumerable<EducationReadDto>>(items);
        }

        public async Task<EducationReadDto> CreateAsync(EducationCreateDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<EducationDto>(dto);

            await _uow.BeginTransactionAsync();
            try
            {
                var id = await _uow.Educations.CreateAsync(entity);
                var created = await _uow.Educations.GetByIdAsync(id);

                await _uow.CommitAsync();
                return _mapper.Map<EducationReadDto>(created);
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task<EducationReadDto> UpdateAsync(int id, EducationUpdateDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var existing = await _uow.Educations.GetByIdAsync(id)
                ?? throw new NotFoundException($"Education {id} not found");

            _mapper.Map(dto, existing);

            await _uow.BeginTransactionAsync();
            try
            {
                await _uow.Educations.UpdateAsync(existing);
                var updated = await _uow.Educations.GetByIdAsync(id);

                await _uow.CommitAsync();
                return _mapper.Map<EducationReadDto>(updated);
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _uow.Educations.GetByIdAsync(id)
                ?? throw new NotFoundException($"Education {id} not found");

            await _uow.BeginTransactionAsync();
            try
            {
                await _uow.Educations.DeleteAsync(id);
                await _uow.CommitAsync();
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
    }
}
