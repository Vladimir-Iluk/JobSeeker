using Bll.Dto.JobSeeker;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Validators.JobSeeker
{
    public class JobSeekerUpdateValidator : AbstractValidator<JobSeekerUpdateDto>
    {
        public JobSeekerUpdateValidator()
        {
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
