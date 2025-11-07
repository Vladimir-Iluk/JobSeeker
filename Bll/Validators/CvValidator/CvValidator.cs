using Bll.Dto.Cv;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bll.Validators.CvValidator
{
    public class CvValidator : AbstractValidator<CvCreateDto>
    {
        public CvValidator()
        {
            RuleFor(x => x.Summary).NotEmpty();
        }
    }
}
