using AutoMapper;
using FluentValidation;
using Bll.Dto.Cv;
using Bll.Exceptions;
using Bll.Interfaces;
using Bll.Validators.CvValidator;
using Dal.Dto;
using Dal.Interfaces;

namespace Bll.Services
{
    public class CvService : ICvService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly CvValidator _createValidator;
        private readonly CvUpdateValidator _updateValidator;

        public CvService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
            _createValidator = new CvValidator();
            _updateValidator = new CvUpdateValidator();
        }

        public async Task<CvReadDto> GetByIdAsync(int id)
        {
            var entity = await _uow.CVs.GetByIdAsync(id)
                ?? throw new NotFoundException($"Cv {id} not found");

            return _mapper.Map<CvReadDto>(entity);
        }

        public async Task<IEnumerable<CvReadDto>> GetByJobSeekerAsync(int jobSeekerId)
        {
            var items = await _uow.CVs.GetByJobSeekerIdAsync(jobSeekerId);
            return _mapper.Map<IEnumerable<CvReadDto>>(items);
        }

        public async Task<CvReadDto> CreateAsync(CvCreateDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<CvDto>(dto);

            await _uow.BeginTransactionAsync();
            try
            {
                var id = await _uow.CVs.CreateAsync(entity);
                var created = await _uow.CVs.GetByIdAsync(id);

                await _uow.CommitAsync();
                return _mapper.Map<CvReadDto>(created);
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task<CvReadDto> UpdateAsync(int id, CvUpdateDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var existing = await _uow.CVs.GetByIdAsync(id)
                ?? throw new NotFoundException($"Cv {id} not found");

            _mapper.Map(dto, existing);

            await _uow.BeginTransactionAsync();
            try
            {
                await _uow.CVs.UpdateAsync(existing);
                var updated = await _uow.CVs.GetByIdAsync(id);

                await _uow.CommitAsync();
                return _mapper.Map<CvReadDto>(updated);
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _uow.CVs.GetByIdAsync(id)
                ?? throw new NotFoundException($"Cv {id} not found");

            await _uow.BeginTransactionAsync();
            try
            {
                await _uow.CVs.DeleteAsync(id);
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
