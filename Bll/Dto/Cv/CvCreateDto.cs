using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Dto.Cv
{
    public class CvCreateDto
    {
        public int JobSeekerId { get; set; }
        public string Summary { get; set; } = null!;
        public string? AdditionalInfo { get; set; }
    }
}
