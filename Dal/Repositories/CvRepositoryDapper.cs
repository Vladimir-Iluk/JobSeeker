using Dal.Dto;
using Dal.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dal.Repositories
{
    public class CvRepository : ICvRepository
    {
        private readonly IDbConnection _connection;

        public CvRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> CreateAsync(CvDto dto, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO cv (jobseekerid, filelink, description)
                VALUES (@JobSeekerId, @FileLink, @Description)
                RETURNING id;";

            var command = new CommandDefinition(sql, dto, cancellationToken: cancellationToken);
            return await _connection.ExecuteScalarAsync<int>(command);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM cv WHERE id = @Id;";
            var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
            await _connection.ExecuteAsync(command);
        }

        public async Task<IEnumerable<CvDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM cv;";
            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
            return await _connection.QueryAsync<CvDto>(command);
        }

        public async Task<CvDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM cv WHERE id = @Id;";
            var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
            return await _connection.QueryFirstOrDefaultAsync<CvDto>(command);
        }

        public async Task<IEnumerable<CvDto>> GetByJobSeekerIdAsync(int jobSeekerId, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM cv WHERE jobseekerid = @JobSeekerId;";
            var command = new CommandDefinition(sql, new { JobSeekerId = jobSeekerId }, cancellationToken: cancellationToken);
            return await _connection.QueryAsync<CvDto>(command);
        }

        public async Task UpdateAsync(CvDto dto, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                UPDATE cv
                SET filelink = @FileLink,
                    description = @Description
                WHERE id = @Id;";

            var command = new CommandDefinition(sql, dto, cancellationToken: cancellationToken);
            await _connection.ExecuteAsync(command);
        }
    }
}
