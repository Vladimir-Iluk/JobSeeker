using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Dto.JobSeeker
{
    public class JobSeekerCreateDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public int? Experience { get; set; }
        public string? Skills { get; set; }
    }
}
