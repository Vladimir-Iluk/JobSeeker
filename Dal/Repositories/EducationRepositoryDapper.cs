using Dal.Dto;
using Dal.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dal.Repositories
{
    public class EducationRepository : IEducationRepository, ITransactionalRepository
    {
        private readonly IDbConnection _connection;
        public IDbTransaction? Transaction { get; set; }

        public EducationRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> CreateAsync(EducationDto dto, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO education (jobseekerid, institution, degree, year)
                VALUES (@JobSeekerId, @Institution, @Degree, @Year)
                RETURNING id;";

            var cmd = new CommandDefinition(sql, dto, Transaction, cancellationToken: cancellationToken);
            return await _connection.ExecuteScalarAsync<int>(cmd);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM education WHERE id = @Id;";

            var cmd = new CommandDefinition(sql, new { Id = id }, Transaction, cancellationToken: cancellationToken);
            await _connection.ExecuteAsync(cmd);
        }

        public async Task<IEnumerable<EducationDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM education;";

            var cmd = new CommandDefinition(sql, transaction: Transaction, cancellationToken: cancellationToken);
            return await _connection.QueryAsync<EducationDto>(cmd);
        }

        public async Task<EducationDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM education WHERE id = @Id;";

            var cmd = new CommandDefinition(sql, new { Id = id }, Transaction, cancellationToken: cancellationToken);
            return await _connection.QueryFirstOrDefaultAsync<EducationDto>(cmd);
        }

        public async Task<IEnumerable<EducationDto>> GetByJobSeekerIdAsync(int jobSeekerId, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM education WHERE jobseekerid = @JobSeekerId;";

            var cmd = new CommandDefinition(sql, new { JobSeekerId = jobSeekerId }, Transaction, cancellationToken: cancellationToken);
            return await _connection.QueryAsync<EducationDto>(cmd);
        }

        public async Task UpdateAsync(EducationDto dto, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                UPDATE education
                SET institution = @Institution,
                    degree = @Degree,
                    year = @Year
                WHERE id = @Id;";

            var cmd = new CommandDefinition(sql, dto, Transaction, cancellationToken: cancellationToken);
            await _connection.ExecuteAsync(cmd);
        }
    }
}
