using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Dto.Experience
{
    public class ExperienceCreateDto
    {
        public int JobSeekerId { get; set; }
        public string Company { get; set; } = null!;
        public string Position { get; set; } = null!;
        public int Years { get; set; }
    }
}
