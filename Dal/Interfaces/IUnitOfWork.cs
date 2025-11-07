using System;
using System.Threading.Tasks;

namespace Dal.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IJobSeekerRepository JobSeekers { get; }
        IEducationRepository Educations { get; }
        IExperienceRecordRepository ExperienceRecords { get; }
        ICvRepository CVs { get; }

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
