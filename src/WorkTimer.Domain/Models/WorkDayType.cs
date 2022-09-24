namespace WorkTimer.Domain.Models;
public enum WorkDayType
{
    Undefined = 0,
    Workday = 1,
    BankHoliday = 2,
    Vacation = 3,
    HalfVacation = 4,
    SickDay = 5,
    ParentalLeave = 6,
    ChildSickDay = 7,
    Weekend = 8
}