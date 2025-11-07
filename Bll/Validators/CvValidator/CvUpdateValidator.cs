using Bll.Dto.Cv;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Validators.CvValidator
{
    public class CvUpdateValidator : AbstractValidator<CvUpdateDto>
    {
        public CvUpdateValidator()
        {
            RuleFor(x => x.Summary).NotEmpty();
        }
    }
}
