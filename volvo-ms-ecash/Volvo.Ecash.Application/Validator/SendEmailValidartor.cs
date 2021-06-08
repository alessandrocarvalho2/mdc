using FluentValidation;
using Volvo.Ecash.Dto.Model;
using System;

namespace Volvo.Ecash.Application.Validator
{

    public class SendEmailValidartor : AbstractValidator<ResetPasswordDto>
    {
        public SendEmailValidartor()
        {
            RuleFor(c => c)
                .NotNull()
                .OnAnyFailure(x =>
                {
                    throw new ArgumentException("Não foi possivel encontrar o objeto.", "x");
                });

            RuleFor(c => c.Login)
                .NotEmpty().WithMessage("É necessário informar o Login.")
                .NotNull().WithMessage("O login não pode ser nulo.");



        }
    }
}
