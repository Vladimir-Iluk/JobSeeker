using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Dto
{
    public class ExperienceRecordDto
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? Position { get; set; }
        public int? Years { get; set; }
    }
}
