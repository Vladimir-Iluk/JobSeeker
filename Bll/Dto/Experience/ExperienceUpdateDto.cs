using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Dto.Experience
{
    public class ExperienceUpdateDto
    {
        public string Company { get; set; } = null!;
        public string Position { get; set; } = null!;
        public int Years { get; set; }
    }
}
