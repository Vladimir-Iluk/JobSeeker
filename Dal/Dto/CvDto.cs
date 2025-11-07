using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Dto
{
    public class CvDto
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public string? FileLink { get; set; }
        public string? Description { get; set; }
    }
}
