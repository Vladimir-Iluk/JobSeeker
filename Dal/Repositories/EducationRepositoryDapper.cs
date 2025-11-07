using Dal.Dto;
using Dal.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dal.Repositories
{
    public class EducationRepository : IEducationRepository
    {
        private readonly IDbConnection _connection;

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

            var command = new CommandDefinition(sql, dto, cancellationToken: cancellationToken);
            return await _connection.ExecuteScalarAsync<int>(command);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM education WHERE id = @Id;";
            var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
            await _connection.ExecuteAsync(command);
        }

        public async Task<IEnumerable<EducationDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM education;";
            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
            return await _connection.QueryAsync<EducationDto>(command);
        }

        public async Task<EducationDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM education WHERE id = @Id;";
            var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
            return await _connection.QueryFirstOrDefaultAsync<EducationDto>(command);
        }

        public async Task<IEnumerable<EducationDto>> GetByJobSeekerIdAsync(int jobSeekerId, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM education WHERE jobseekerid = @JobSeekerId;";
            var command = new CommandDefinition(sql, new { JobSeekerId = jobSeekerId }, cancellationToken: cancellationToken);
            return await _connection.QueryAsync<EducationDto>(command);
        }

        public async Task UpdateAsync(EducationDto dto, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                UPDATE education
                SET institution = @Institution,
                    degree = @Degree,
                    year = @Year
                WHERE id = @Id;";

            var command = new CommandDefinition(sql, dto, cancellationToken: cancellationToken);
            await _connection.ExecuteAsync(command);
        }
    }
}
