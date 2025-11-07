using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Dto.Cv
{
    public class CvUpdateDto
    {
        public string Summary { get; set; } = null!;
        public string? AdditionalInfo { get; set; }
    }
}
