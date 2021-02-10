using Microsoft.EntityFrameworkCore;
using System.Linq;
using WorkTimer.Domain.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers.Shared {
    public class TotalHoursBase {
        protected readonly AppDbContext _context;

        public TotalHoursBase(AppDbContext context) {
            _context = context;
        }

        protected void UpdateTotalHoursOfWorkDay(int workdayId) {
            WorkDay? workday = _context.WorkDays.Include(x => x.WorkingPeriods)
                .Where(x => x.Id == workdayId && x.WorkingPeriods.Any(wp => wp.EndTime.HasValue))
                .Select(x => x)
                .FirstOrDefault();

            UpdateTotalHoursOfWorkDay(workday);
        }

        protected void UpdateTotalHoursOfWorkDay(WorkDay workday) {
            if (workday?.WorkingPeriods.Count > 0) {
                double totalHours = workday.WorkingPeriods.Where(x => x.EndTime.HasValue).Sum(x => (x.EndTime.Value - x.StartTime).TotalHours);
                workday.TotalHours = totalHours;
                _context.WorkDays.Update(workday);
            }
        }
    }
}