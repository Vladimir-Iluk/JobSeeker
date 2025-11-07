using Bll.Dto.Education;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Validators.Education
{
    public class EducationValidator : AbstractValidator<EducationCreateDto>
    {
        public EducationValidator()
        {
            RuleFor(x => x.Institution).NotEmpty();
            RuleFor(x => x.Year).InclusiveBetween(1900, DateTime.Now.Year);
        }
    }
}
