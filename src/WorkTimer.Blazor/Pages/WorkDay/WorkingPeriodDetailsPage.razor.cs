using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;
using WorkTimer.MediatR.Handlers;

namespace WorkTimer.Blazor.Pages.WorkDay;

public partial class WorkingPeriodDetailsPage
{
    [Parameter]
    public int WorkDayId { get; set; }

    [Parameter]
    public int WorkingPeriodId { get; set; }

    [Inject]
    public IDialogService DialogService { get; set; } = default!;

    [Inject]
    public IMediator Mediator { get; set; } = default!;

    [Inject]
    public NavigationManager Navi { get; set; } = default!;

    public GetWorkingPeriodResponse Model { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Model = await Mediator.Send(new GetWorkingPeriodRequest(WorkDayId, WorkingPeriodId));
    }

    protected async Task HandleValidSubmitAsync()
    {
        Model.WorkingPeriod.StartTime = Model.StartDate!.Value.Add(Model.StartTime!.Value);

        if (Model.EndDate is not null && Model.EndTime is not null)
        {
            Model.WorkingPeriod.EndTime = Model.EndDate!.Value.Add(Model.EndTime!.Value);
        }
        else
        {
            Model.WorkingPeriod.EndTime = null;
        }

        await Mediator.Send(Model);

        Navi.NavigateTo($"/workday/{Model.WorkingPeriod.WorkDayId}");
    }

    protected string GetEndTimeForTitle()
    {
        return Model.WorkingPeriod.EndTime.HasValue ? " - " + Model.WorkingPeriod.EndTime.Value : "";
    }

    void OpenDialog()
    {
        DialogParameters parameters = new()
        {
            { "DeleteEntity", OkClickAsync },
        };

        DialogService.Show<ConfirmDeletionDialog>("Delete", parameters);
    }

    async Task OkClickAsync()
    {
        var result = await Mediator.Send(new DeleteWorkingPeriodRequest(Model.WorkingPeriod) { User = Model.UserContext.User });

        if (result)
        {
            Navi.NavigateTo($"workday/{Model.WorkingPeriod.WorkDayId}");
        }
    }
}