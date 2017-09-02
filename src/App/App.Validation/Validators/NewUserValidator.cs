using System.Diagnostics.CodeAnalysis;
using App.Dto.Request;
using App.Services.Contracts;
using FluentValidation;

namespace App.Validation.Validators
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class NewUserValidator : NullReferenceAbstractValidator<NewUserDto>
    {
        public NewUserValidator(IUserService userService)
        {
            RuleFor(model => model.FullName).NotEmpty();
            RuleFor(model => model.Email).NotEmpty().EmailAddress();

            var message = "The email address not is available";
            RuleFor(model => model.Email)
                .NotEmpty()
                .Must(x => !userService.EmailAlreadyRegistered(x))
                .WithMessage(message);
            RuleFor(x => x.Password)
                .NotNull().WithMessage("Password is required");
        }
    }
}
