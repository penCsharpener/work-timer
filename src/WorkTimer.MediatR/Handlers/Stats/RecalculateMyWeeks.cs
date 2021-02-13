using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Extensions;
using WorkTimer.MediatR.Models;
using WorkTimer.MediatR.Specifications;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers.Stats
{
    public class RecalculateMyWeeksRequest : UserContext, IRequest<RecalculateMyWeeksResponse>
    {
        private readonly int _year;
        private readonly int _calendarWeek;
        private readonly DateTime _date;

        private static Calendar _calendar = new CultureInfo("de-DE").Calendar;

        public RecalculateMyWeeksRequest() { }

        public RecalculateMyWeeksRequest(int year, int calendarWeek)
        {
            _year = year;
            _calendarWeek = calendarWeek;

            bool foundWeek;
            var date = new DateTime(year, 01, 01);

            while (_calendarWeek != _calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday))
            {
                date = date.AddDays(7);
            }

            CalendarWeek = _calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            DaysInWeek = date.GetWholeWeek().ToList();
        }

        public RecalculateMyWeeksRequest(DateTime date)
        {
            _date = date;

            CalendarWeek = _calendar.GetWeekOfYear(_date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            DaysInWeek = _date.GetWholeWeek().ToList();
        }

        public int CalendarWeek { get; }
        public List<DateTime> DaysInWeek { get; }
    }

    public class RecalculateMyWeeksResponse
    {

    }

    public class RecalculateMyWeeksHandler : IRequestHandler<RecalculateMyWeeksRequest, RecalculateMyWeeksResponse>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RecalculateMyWeeksHandler> _logger;

        public RecalculateMyWeeksHandler(AppDbContext context, ILogger<RecalculateMyWeeksHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<RecalculateMyWeeksResponse> Handle(RecalculateMyWeeksRequest request, CancellationToken cancellationToken)
        {
            var response = new RecalculateMyWeeksResponse();

            var workDays = await _context.WorkDays.Include(x => x.WorkWeek)
                                    .Where(new RecalculateMyWeeksSpecification(request.User.Id, request.DaysInWeek).ToExpression())
                                    .ToListAsync();

            return response;
        }
    }
}
