using Dal.Dto;
using Dal.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dal.Repositories
{
    public class JobSeekerRepositoryAdo : IJobSeekerRepository
    {
        private readonly NpgsqlConnection _connection;

        public JobSeekerRepositoryAdo(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        private async Task EnsureOpenAsync()
        {
            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();
        }

        public async Task<JobSeekerDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            await EnsureOpenAsync();

            const string sql = "SELECT * FROM jobseekers WHERE id = @id";

            using var command = new NpgsqlCommand(sql, _connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if (!await reader.ReadAsync(cancellationToken))
                return null;

            return new JobSeekerDto
            {
                Id = reader.GetInt32(0),
                FullName = reader.GetString(1),
                Email = reader.GetString(2),
                Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                Experience = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                Skills = reader.IsDBNull(5) ? null : reader.GetString(5)
            };
        }

        public async Task<IEnumerable<JobSeekerDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            await EnsureOpenAsync();
            var result = new List<JobSeekerDto>();

            const string sql = "SELECT * FROM jobseekers";

            using var command = new NpgsqlCommand(sql, _connection);
            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                result.Add(new JobSeekerDto
                {
                    Id = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Email = reader.GetString(2),
                    Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Experience = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                    Skills = reader.IsDBNull(5) ? null : reader.GetString(5)
                });
            }

            return result;
        }

        public async Task<int> CreateAsync(JobSeekerDto dto, CancellationToken cancellationToken = default)
        {
            await EnsureOpenAsync();

            const string sql = @"
                INSERT INTO jobseekers (fullname, email, phone, experience, skills)
                VALUES (@fullname, @email, @phone, @experience, @skills)
                RETURNING id;";

            using var command = new NpgsqlCommand(sql, _connection);

            command.Parameters.AddWithValue("@fullname", dto.FullName);
            command.Parameters.AddWithValue("@email", dto.Email);
            command.Parameters.AddWithValue("@phone", (object?)dto.Phone ?? DBNull.Value);
            command.Parameters.AddWithValue("@experience", (object?)dto.Experience ?? DBNull.Value);
            command.Parameters.AddWithValue("@skills", (object?)dto.Skills ?? DBNull.Value);

            var id = (int)await command.ExecuteScalarAsync(cancellationToken);
            return id;
        }

        public async Task UpdateAsync(JobSeekerDto dto, CancellationToken cancellationToken = default)
        {
            await EnsureOpenAsync();

            const string sql = @"
                UPDATE jobseekers
                SET fullname = @fullname, email = @email, phone = @phone, experience = @experience, skills = @skills
                WHERE id = @id;";

            using var command = new NpgsqlCommand(sql, _connection);

            command.Parameters.AddWithValue("@id", dto.Id);
            command.Parameters.AddWithValue("@fullname", dto.FullName);
            command.Parameters.AddWithValue("@email", dto.Email);
            command.Parameters.AddWithValue("@phone", (object?)dto.Phone ?? DBNull.Value);
            command.Parameters.AddWithValue("@experience", (object?)dto.Experience ?? DBNull.Value);
            command.Parameters.AddWithValue("@skills", (object?)dto.Skills ?? DBNull.Value);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await EnsureOpenAsync();

            const string sql = "DELETE FROM jobseekers WHERE id = @id";

            using var command = new NpgsqlCommand(sql, _connection);
            command.Parameters.AddWithValue("@id", id);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        public async Task<IEnumerable<JobSeekerDto>> SearchByNameOrSkillAsync(string query, CancellationToken cancellationToken = default)
        {
            await EnsureOpenAsync();
            var result = new List<JobSeekerDto>();

            const string sql = @"SELECT * FROM jobseekers WHERE fullname ILIKE @q OR skills ILIKE @q";

            using var command = new NpgsqlCommand(sql, _connection);
            command.Parameters.AddWithValue("@q", $"%{query}%");

            using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                result.Add(new JobSeekerDto
                {
                    Id = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Email = reader.GetString(2),
                    Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Experience = reader.IsDBNull(4) ? null : reader.GetInt32(4),
                    Skills = reader.IsDBNull(5) ? null : reader.GetString(5)
                });
            }

            return result;
        }
    }
}
