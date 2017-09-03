using System.Diagnostics.CodeAnalysis;
using App.Dto.Request;
using App.Services.Contracts;
using FluentValidation;

namespace App.Validation.Validators
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class NewUserValidator : NullReferenceAbstractValidator<NewUserDto>
    {
        private const string EmailNotAvailable = "The email address not is available";
        private const string UserNameNotAvailable = "The username is not available";

        public NewUserValidator(IUserService userService)
        {
            RuleFor(model => model.Description).NotEmpty();
            RuleFor(model => model.FullName).NotEmpty();

            RuleFor(model => model.UserName)
                .NotEmpty()
                .Must(x => !userService.UsernameAlreadyRegistered(x))
                .WithMessage(UserNameNotAvailable);

            RuleFor(model => model.Email)
                .NotEmpty()
                .EmailAddress()
                .Must(x => !userService.EmailAlreadyRegistered(x))
                .WithMessage(EmailNotAvailable);

            RuleFor(x => x.Password)
                .NotNull().WithMessage("Password is required");
        }
    }
}
