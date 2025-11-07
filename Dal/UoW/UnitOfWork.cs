using Dal.Database;
using Dal.Interfaces;
using Dal.Repositories;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dal.Interfaces;
using Npgsql;
using System.Data;
using System.Threading.Tasks;
using Dal.Repositories;
namespace Dal.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NpgsqlConnection _connection;
        private NpgsqlTransaction? _transaction;

        public IJobSeekerRepository JobSeekers { get; }
        public IEducationRepository Educations { get; }
        public IExperienceRecordRepository ExperienceRecords { get; }
        public ICvRepository CVs { get; }

        public UnitOfWork(DapperContext context)
        {
            _connection = context.CreateConnection() as NpgsqlConnection
                ?? throw new InvalidOperationException("Не вдалося створити підключення PostgreSQL.");

            // Репозиторії з reuse одного з’єднання
            JobSeekers = new JobSeekerRepositoryAdo(_connection);
            Educations = new EducationRepository(_connection);
            ExperienceRecords = new ExperienceRecordRepository(_connection);
            CVs = new CvRepository(_connection);
        }

        public async Task BeginTransactionAsync()
        {
            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();

            _transaction = await _connection.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection.Dispose();
        }
    }
}
