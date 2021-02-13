namespace WorkTimer.MediatR.Handlers
{
    internal record TotalHoursCalculationModel(double TotalHours, double HoursPerDay, double WorkHourMultiplier);
}