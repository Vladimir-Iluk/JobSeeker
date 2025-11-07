using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Entities
{
    public class Education
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public string Institution { get; set; } = null!;
        public string? Degree { get; set; }
        public int? Year { get; set; }
        public JobSeeker? JobSeeker { get; set; }
    }
}
