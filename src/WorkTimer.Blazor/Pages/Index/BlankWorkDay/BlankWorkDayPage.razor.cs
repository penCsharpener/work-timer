using System;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Handlers;

namespace WorkTimer.Blazor.Pages.Index.BlankWorkDay;

public partial class BlankWorkDayPage
{
    public CreateBlankWorkDayCommand Model { get; set; } = new(DateTime.Today.AddDays(1));
    public WorkDayType[] WorkDayTypes { get; set; } = Enum.GetValues(typeof(WorkDayType)).Cast<WorkDayType>().ToArray();
    private async Task HandleValidSubmitAsync()
    {
        var response = await Mediator.Send(Model);
        Navi.NavigateTo($"workday/{response.WorkDay.Id}");
    }
}