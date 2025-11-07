
using Dal.Database;
using Dal.Interfaces;
using Dal.Repositories;
using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;

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
            PropagateTransactionToRepositories(_transaction);
        }

        private void PropagateTransactionToRepositories(IDbTransaction? tx)
        {
            void TrySet(object repo)
            {
                if (repo is ITransactionalRepository t)
                    t.Transaction = tx;
            }

            TrySet(JobSeekers);
            TrySet(Educations);
            TrySet(ExperienceRecords);
            TrySet(CVs);
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                _transaction.Dispose();
                _transaction = null;
                PropagateTransactionToRepositories(null);
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                _transaction.Dispose();
                _transaction = null;
                PropagateTransactionToRepositories(null);
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection.Dispose();
        }
    }
}
