using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Entities
{
    public class ExperienceRecord
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? Position { get; set; }
        public int? Years { get; set; }

        public JobSeeker? JobSeeker { get; set; }
    }
}
