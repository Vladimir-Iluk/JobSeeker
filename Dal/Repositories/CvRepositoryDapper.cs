using Dal.Dto;
using Dal.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dal.Repositories
{
    public class CvRepository : ICvRepository, ITransactionalRepository
    {
        private readonly IDbConnection _connection;
        public IDbTransaction? Transaction { get; set; }

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

            var cmd = new CommandDefinition(
                sql,
                dto,
                Transaction,
                cancellationToken: cancellationToken
            );

            return await _connection.ExecuteScalarAsync<int>(cmd);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM cv WHERE id = @Id;";

            var cmd = new CommandDefinition(
                sql,
                new { Id = id },
                Transaction,
                cancellationToken: cancellationToken
            );

            await _connection.ExecuteAsync(cmd);
        }

        public async Task<IEnumerable<CvDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM cv;";

            var cmd = new CommandDefinition(
                sql,
                transaction: Transaction,
                cancellationToken: cancellationToken
            );

            return await _connection.QueryAsync<CvDto>(cmd);
        }

        public async Task<CvDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM cv WHERE id = @Id;";

            var cmd = new CommandDefinition(
                sql,
                new { Id = id },
                Transaction,
                cancellationToken: cancellationToken
            );

            return await _connection.QueryFirstOrDefaultAsync<CvDto>(cmd);
        }

        public async Task<IEnumerable<CvDto>> GetByJobSeekerIdAsync(int jobSeekerId, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM cv WHERE jobseekerid = @JobSeekerId;";

            var cmd = new CommandDefinition(
                sql,
                new { JobSeekerId = jobSeekerId },
                Transaction,
                cancellationToken: cancellationToken
            );

            return await _connection.QueryAsync<CvDto>(cmd);
        }

        public async Task UpdateAsync(CvDto dto, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                UPDATE cv
                SET filelink = @FileLink,
                    description = @Description
                WHERE id = @Id;";

            var cmd = new CommandDefinition(
                sql,
                dto,
                Transaction,
                cancellationToken: cancellationToken
            );

            await _connection.ExecuteAsync(cmd);
        }
    }
}
