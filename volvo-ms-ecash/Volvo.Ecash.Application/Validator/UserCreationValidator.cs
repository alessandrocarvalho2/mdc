using FluentValidation;
using Volvo.Ecash.Dto.Model;
using System;
using System.Text.RegularExpressions;
using Volvo.Ecash.Dto.Enum;

namespace Volvo.Ecash.Application.Validator
{
    public class UserCreationValidator : AbstractValidator<LoginCreationDto>
    {
        public UserCreationValidator()
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

            RuleFor(c => c.Name)
               .NotEmpty().WithMessage("É necessário informar o campo nome.")
                .NotNull().WithMessage("O campo login não pode ser nome.");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("É necessário informar o campo senha.")
                .NotNull().WithMessage("O campo senha não pode ser nula.");

            //When(p => p.ProfileCode != ProfileType.ALU.ToString() && p.ProfileCode != ProfileType.RESP.ToString(), () => RuleFor(c => c.Email)
            //          .EmailAddress().WithMessage("Favor informar um e-mail válido .")
            //          .NotEmpty().WithMessage("Favor informar o campo e-mail.")
            //          .NotNull().WithMessage("Favor informar o campo e-mail."));


            RuleFor(x => x.Email).Custom((email, context) =>
            {

                if (!string.IsNullOrEmpty(email) && !IsValid(email))
                {
                    context.AddFailure("Favor informar um e-mail válido .");
                }

            });

            RuleFor(x => x.Password).Custom((password, context) =>
            {

                bool noSpecialChars = Regex.IsMatch(password, "^[a-zA-Z0-9@-_.]*$");
                bool containsNumber = Regex.IsMatch(password, @"\d");
                bool containsletters = Regex.IsMatch(password, "[a-zA-Z]");
                var passwordDefault = string.Format("{0}@{1}", "ecash", DateTime.Now.Year);

                if (password != passwordDefault)
                {
                    if (!containsNumber || !containsletters || !noSpecialChars)
                    {
                        context.AddFailure("A senha deve conter letras e números.");
                    }

                    if (password.Length > 8)
                    {
                        context.AddFailure("A senha não pode conter mais que 8 caracteres.");
                    }

                    if (password.Length < 6)
                    {
                        context.AddFailure("A senha deve conter no mínimo 6 caracteres.");
                    }
                }

            });



        }

        public bool IsValid(string emailaddress)
        {
            return Regex.IsMatch(emailaddress, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

        }
    }
}
