using Bll.Dto.JobSeeker;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Bll.Validators.JobSeeker
{
    public class JobSeekerValidator : AbstractValidator<JobSeekerCreateDto>
    {
        public JobSeekerValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
