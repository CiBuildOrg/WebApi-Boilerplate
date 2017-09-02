using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace App.Validation
{
    public class NullReferenceAbstractValidator<T> : AbstractValidator<T>
    {
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            return context.InstanceToValidate == null
                ? new ValidationResult(new[] {new ValidationFailure(typeof(T).Name, "request cannot be null", "Error")})
                : base.Validate(context);
        }

        public override Task<ValidationResult> ValidateAsync(ValidationContext<T> context,
            CancellationToken cancellation = new CancellationToken())
        {
            return context.InstanceToValidate == null
                ? Task.FromResult(new ValidationResult(new[]
                    {new ValidationFailure(typeof(T).Name, "request cannot be null", "Error")}))
                : base.ValidateAsync(context, cancellation);
        }
    }
}
