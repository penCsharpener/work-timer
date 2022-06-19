using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WorkTimer.Domain.Models;

namespace WorkTimer.MediatR.Specifications;

public class RecalculateMyWeeksSpecification
{
    private readonly Expression<Func<WorkDay, bool>> _expression;

    public RecalculateMyWeeksSpecification(int userId)
    {
        _expression = x => x.Contract.UserId == userId;
    }

    public RecalculateMyWeeksSpecification(int userId, List<DateTime> daysInWeek)
    {
        _expression = x => daysInWeek != null && daysInWeek.Count == 7 && daysInWeek.Contains(x.Date) && x.Contract.UserId == userId;
    }

    public Expression<Func<WorkDay, bool>> ToExpression()
    {
        return _expression;
    }
}
