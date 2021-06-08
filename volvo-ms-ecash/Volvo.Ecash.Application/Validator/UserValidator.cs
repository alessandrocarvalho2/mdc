using System;
using System.Text.RegularExpressions;
using FluentValidation;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Validator
{
    public class UserValidator : AbstractValidator<UserLogin>
    {
        public UserValidator()
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

            RuleFor(c => c.Password.Length)
                .LessThan(18)
                .WithMessage("A senha não pode conter mais que 18 caracteres.");

            RuleFor(c => c.Password.Length)
                .GreaterThan(5)
                .WithMessage("A senha deve conter no mínimo  6 caracteres.");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("É necessário informar o campo senha.")
                .NotNull().WithMessage("O campo senha não pode ser nulo.");

            RuleFor(x => x.Password).Custom((password, context) =>
            {

                bool containsNumber = Regex.IsMatch(password, @"\d");

                bool containsletters = Regex.IsMatch(password, "[a-zA-Z]");

                if (!containsNumber || !containsletters)
                {
                    context.AddFailure("A senha deve conter letras e números.");
                }

            });

        }
    }
}
