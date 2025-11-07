using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Dto.Education
{
    public class EducationReadDto
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public string Institution { get; set; } = null!;
        public string Degree { get; set; } = null!;
        public int Year { get; set; }
    }
}
