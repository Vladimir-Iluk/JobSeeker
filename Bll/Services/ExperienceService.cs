using AutoMapper;
using FluentValidation;
using Bll.Dto.Experience;
using Bll.Exceptions;
using Bll.Interfaces;
using Bll.Validators.ExperienceValidator;
using Dal.Dto;
using Dal.Interfaces;

namespace Bll.Services
{
    public class ExperienceService : IExperienceService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ExperienceValidator _createValidator;
        private readonly ExperienceUpdateValidator _updateValidator;

        public ExperienceService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
            _createValidator = new ExperienceValidator();
            _updateValidator = new ExperienceUpdateValidator();
        }

        public async Task<ExperienceReadDto> GetByIdAsync(int id)
        {
            var entity = await _uow.ExperienceRecords.GetByIdAsync(id)
                ?? throw new NotFoundException($"Experience {id} not found");

            return _mapper.Map<ExperienceReadDto>(entity);
        }

        public async Task<IEnumerable<ExperienceReadDto>> GetByJobSeekerAsync(int jobSeekerId)
        {
            var items = await _uow.ExperienceRecords.GetByJobSeekerIdAsync(jobSeekerId);
            return _mapper.Map<IEnumerable<ExperienceReadDto>>(items);
        }

        public async Task<ExperienceReadDto> CreateAsync(ExperienceCreateDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<ExperienceRecordDto>(dto);

            await _uow.BeginTransactionAsync();
            try
            {
                var id = await _uow.ExperienceRecords.CreateAsync(entity);
                var created = await _uow.ExperienceRecords.GetByIdAsync(id);

                await _uow.CommitAsync();
                return _mapper.Map<ExperienceReadDto>(created);
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task<ExperienceReadDto> UpdateAsync(int id, ExperienceUpdateDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var existing = await _uow.ExperienceRecords.GetByIdAsync(id)
                ?? throw new NotFoundException($"Experience {id} not found");

            _mapper.Map(dto, existing);

            await _uow.BeginTransactionAsync();
            try
            {
                await _uow.ExperienceRecords.UpdateAsync(existing);
                var updated = await _uow.ExperienceRecords.GetByIdAsync(id);

                await _uow.CommitAsync();
                return _mapper.Map<ExperienceReadDto>(updated);
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _uow.ExperienceRecords.GetByIdAsync(id)
                ?? throw new NotFoundException($"Experience {id} not found");

            await _uow.BeginTransactionAsync();
            try
            {
                await _uow.ExperienceRecords.DeleteAsync(id);
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
