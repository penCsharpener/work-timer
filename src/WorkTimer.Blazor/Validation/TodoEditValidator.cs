using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;

namespace WorkTimer.Blazor.Validation;

public class TodoEditValidator : AbstractValidator<Todo>
{
    public TodoEditValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(1, 150);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<Todo>.CreateWithOptions((Todo) model, x => x.IncludeProperties(propertyName)));

        return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
    };
}