using Bll.Dto.Experience;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Validators.ExperienceValidator
{
    public class ExperienceUpdateValidator : AbstractValidator<ExperienceUpdateDto>
    {
        public ExperienceUpdateValidator()
        {
            RuleFor(x => x.Company).NotEmpty();
            RuleFor(x => x.Years).InclusiveBetween(0, 50);
        }
    }
}
