using System;
using FluentValidation;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Validator
{
    public class UserTokenValidator : AbstractValidator<UserDto>
    {
        public UserTokenValidator()
        {
            RuleFor(c => c)
                .NotNull()
                .OnAnyFailure(x =>
                {
                    throw new ArgumentException("Can't found the object.", "x");
                });

            RuleFor(c => c.Login)
               .NotEmpty().WithMessage("É necessário informar o campo login.")
                .NotNull().WithMessage("O campo login não pode ser nulo.");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("É necessário informar o campo senha.")
                .NotNull().WithMessage("O campo senha não pode ser nulo.");

        }
    }

    public class UserLoginTokenValidator : AbstractValidator<UserLogin>
    {
        public UserLoginTokenValidator()
        {
            RuleFor(c => c)
                .NotNull()
                .OnAnyFailure(x =>
                {
                    throw new ArgumentException("Can't found the object.", "x");
                });

            RuleFor(c => c.Login)
               .NotEmpty().WithMessage("É necessário informar o campo login.")
                .NotNull().WithMessage("O campo login não pode ser nulo.");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("É necessário informar o campo senha.")
                .NotNull().WithMessage("O campo senha não pode ser nulo.");

        }
    }
}
