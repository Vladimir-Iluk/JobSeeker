using Bll.Dto.Experience;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Validators.ExperienceValidator
{
    public class ExperienceValidator : AbstractValidator<ExperienceCreateDto>
    {
        public ExperienceValidator()
        {
            RuleFor(x => x.Company).NotEmpty();
            RuleFor(x => x.Years).InclusiveBetween(0, 50);
        }
    }
}
