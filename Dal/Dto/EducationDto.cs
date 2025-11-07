using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Dto
{
    public class EducationDto
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public string Institution { get; set; } = null!;
        public string? Degree { get; set; }
        public int? Year { get; set; }
    }
}
