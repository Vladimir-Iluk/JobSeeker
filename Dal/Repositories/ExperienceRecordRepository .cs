using Dal.Dto;
using Dal.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dal.Repositories
{
    public class ExperienceRecordRepository : IExperienceRecordRepository
    {
        private readonly IDbConnection _connection;

        public ExperienceRecordRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> CreateAsync(ExperienceRecordDto dto, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO experiencerecord (jobseekerid, companyname, position, years)
                VALUES (@JobSeekerId, @CompanyName, @Position, @Years)
                RETURNING id;";

            var command = new CommandDefinition(sql, dto, cancellationToken: cancellationToken);
            return await _connection.ExecuteScalarAsync<int>(command);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM experiencerecord WHERE id = @Id;";
            var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
            await _connection.ExecuteAsync(command);
        }

        public async Task<IEnumerable<ExperienceRecordDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM experiencerecord;";
            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
            return await _connection.QueryAsync<ExperienceRecordDto>(command);
        }

        public async Task<ExperienceRecordDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM experiencerecord WHERE id = @Id;";
            var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
            return await _connection.QueryFirstOrDefaultAsync<ExperienceRecordDto>(command);
        }

        public async Task<IEnumerable<ExperienceRecordDto>> GetByJobSeekerIdAsync(int jobSeekerId, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM experiencerecord WHERE jobseekerid = @JobSeekerId;";
            var command = new CommandDefinition(sql, new { JobSeekerId = jobSeekerId }, cancellationToken: cancellationToken);
            return await _connection.QueryAsync<ExperienceRecordDto>(command);
        }

        public async Task UpdateAsync(ExperienceRecordDto dto, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                UPDATE experiencerecord
                SET companyname = @CompanyName,
                    position = @Position,
                    years = @Years
                WHERE id = @Id;";

            var command = new CommandDefinition(sql, dto, cancellationToken: cancellationToken);
            await _connection.ExecuteAsync(command);
        }
    }
}
