using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace WorkTimer.Blazor.Validation;

public class PasswordLengthAttribute : StringLengthAttribute
{
    public PasswordLengthAttribute() : base(int.MaxValue) { }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var passwordOptions = validationContext.GetRequiredService<PasswordOptions>();

        if (value is string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return new ValidationResult($"The password cannot be empty.");
            }

            if (s.Length < passwordOptions.RequiredLength)
            {
                return new ValidationResult($"The password must be at least {passwordOptions.RequiredLength} characters long.");
            }
        }

        return base.IsValid(value, validationContext);
    }
}
