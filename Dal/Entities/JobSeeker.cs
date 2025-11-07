using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Entities
{
    public class JobSeeker
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public int? Experience { get; set; }
        public string? Skills { get; set; }

        public ICollection<Education>? Educations { get; set; }
        public ICollection<ExperienceRecord>? ExperienceRecords { get; set; }
        public ICollection<CV>? CVs { get; set; }
    }
}
