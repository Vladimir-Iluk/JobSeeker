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
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();

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
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();

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
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();

            const string sql = "SELECT id, jobseekerid as JobSeekerId, filelink as FileLink, description as Description FROM cv;";

            var cmd = new CommandDefinition(
                sql,
                transaction: Transaction,
                cancellationToken: cancellationToken
            );

            return await _connection.QueryAsync<CvDto>(cmd);
        }

        public async Task<CvDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();

            const string sql = "SELECT id, jobseekerid as JobSeekerId, filelink as FileLink, description as Description FROM cv WHERE id = @Id;";

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
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();

            const string sql = "SELECT id, jobseekerid as JobSeekerId, filelink as FileLink, description as Description FROM cv WHERE jobseekerid = @JobSeekerId;";

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
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();

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
