using Dal.Dto;
using Dal.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dal.Repositories
{
    public class ExperienceRecordRepository : IExperienceRecordRepository, ITransactionalRepository
    {
        private readonly IDbConnection _connection;
        public IDbTransaction? Transaction { get; set; }

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

            var cmd = new CommandDefinition(sql, dto, Transaction, cancellationToken: cancellationToken);
            return await _connection.ExecuteScalarAsync<int>(cmd);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM experiencerecord WHERE id = @Id;";

            var cmd = new CommandDefinition(sql, new { Id = id }, Transaction, cancellationToken: cancellationToken);
            await _connection.ExecuteAsync(cmd);
        }

        public async Task<IEnumerable<ExperienceRecordDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM experiencerecord;";

            var cmd = new CommandDefinition(sql, transaction: Transaction, cancellationToken: cancellationToken);
            return await _connection.QueryAsync<ExperienceRecordDto>(cmd);
        }

        public async Task<ExperienceRecordDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM experiencerecord WHERE id = @Id;";

            var cmd = new CommandDefinition(sql, new { Id = id }, Transaction, cancellationToken: cancellationToken);
            return await _connection.QueryFirstOrDefaultAsync<ExperienceRecordDto>(cmd);
        }

        public async Task<IEnumerable<ExperienceRecordDto>> GetByJobSeekerIdAsync(int jobSeekerId, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM experiencerecord WHERE jobseekerid = @JobSeekerId;";

            var cmd = new CommandDefinition(sql, new { JobSeekerId = jobSeekerId }, Transaction, cancellationToken: cancellationToken);
            return await _connection.QueryAsync<ExperienceRecordDto>(cmd);
        }

        public async Task UpdateAsync(ExperienceRecordDto dto, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                UPDATE experiencerecord
                SET companyname = @CompanyName,
                    position = @Position,
                    years = @Years
                WHERE id = @Id;";

            var cmd = new CommandDefinition(sql, dto, Transaction, cancellationToken: cancellationToken);
            await _connection.ExecuteAsync(cmd);
        }
    }
}
