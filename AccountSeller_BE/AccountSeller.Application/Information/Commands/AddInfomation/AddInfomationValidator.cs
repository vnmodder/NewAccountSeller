using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountSeller.Application.Information.Commands.AddInfomation
{
    public class AddInfomationValidator : AbstractValidator<AddInfomationCommand>
    {
        public AddInfomationValidator()
        {
            RuleFor(x => x.FullName).NotNull();
        }
    }
}
