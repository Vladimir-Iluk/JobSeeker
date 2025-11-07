using AutoMapper;
using Bll.Dto.JobSeeker;
using Bll.Exceptions;
using Bll.Interfaces;
using Bll.Validators.JobSeeker;
using Dal.Dto;
using FluentValidation;
using Dal.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Services
{
    public class JobSeekerService : IJobSeekerService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly JobSeekerValidator _createValidator;
        private readonly JobSeekerUpdateValidator _updateValidator;

        public JobSeekerService(
            IUnitOfWork uow,
            IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
            _createValidator = new JobSeekerValidator();
            _updateValidator = new JobSeekerUpdateValidator();
        }

        public async Task<JobSeekerReadDto> GetByIdAsync(int id)
        {
            var entity = await _uow.JobSeekers.GetByIdAsync(id)
                ?? throw new NotFoundException($"JobSeeker {id} not found");

            return _mapper.Map<JobSeekerReadDto>(entity);
        }

        public async Task<IEnumerable<JobSeekerReadDto>> GetAllAsync()
        {
            var list = await _uow.JobSeekers.GetAllAsync();
            return _mapper.Map<IEnumerable<JobSeekerReadDto>>(list);
        }

        public async Task<JobSeekerReadDto> CreateAsync(JobSeekerCreateDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var entity = _mapper.Map<JobSeekerDto>(dto);

            await _uow.BeginTransactionAsync();
            try
            {
                var id = await _uow.JobSeekers.CreateAsync(entity);
                var created = await _uow.JobSeekers.GetByIdAsync(id)
                    ?? throw new BusinessConflictException("Entity created but cannot be fetched");

                await _uow.CommitAsync();
                return _mapper.Map<JobSeekerReadDto>(created);
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task<JobSeekerReadDto> UpdateAsync(int id, JobSeekerUpdateDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var existing = await _uow.JobSeekers.GetByIdAsync(id)
                ?? throw new NotFoundException($"JobSeeker {id} not found");

            _mapper.Map(dto, existing);

            await _uow.BeginTransactionAsync();
            try
            {
                await _uow.JobSeekers.UpdateAsync(existing);
                var updated = await _uow.JobSeekers.GetByIdAsync(id)
                    ?? throw new BusinessConflictException("Entity updated but cannot be fetched");

                await _uow.CommitAsync();
                return _mapper.Map<JobSeekerReadDto>(updated);
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _uow.JobSeekers.GetByIdAsync(id)
                ?? throw new NotFoundException($"JobSeeker {id} not found");

            await _uow.BeginTransactionAsync();
            try
            {
                await _uow.JobSeekers.DeleteAsync(id);
                await _uow.CommitAsync();
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<JobSeekerReadDto>> SearchAsync(string query)
        {
            var result = await _uow.JobSeekers.SearchByNameOrSkillAsync(query);
            return _mapper.Map<IEnumerable<JobSeekerReadDto>>(result);
        }
    }
}
