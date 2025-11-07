using System;
using System.Collections.Generic;
using System.Text;

namespace Dal.Entities
{
    public class CV
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public string? FileLink { get; set; }
        public string? Description { get; set; }

        public JobSeeker? JobSeeker { get; set; }
    }
}
