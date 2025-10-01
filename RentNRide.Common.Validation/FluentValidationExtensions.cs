using FluentValidation;
using RentNRide.Common.Domain.Exceptions;

namespace RentNRide.Common.Validation;

public static class FluentValidationExtension
{
    public static async Task Run<T>(this AbstractValidator<T> validator, T instance)
    {
        var res = await validator.ValidateAsync(instance);

        if (!res.IsValid)
        {
            var failures = res
                .Errors
                .Select(e => e.ErrorMessage)
                .ToList();

            throw new ModelValidationException(failures);
        }
    }
}