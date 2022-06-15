using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;
using WorkTimer.MediatR.Requests;
using WorkTimer.MediatR.Responses;

namespace WorkTimer.Blazor.Pages.WorkDay;

public partial class WorkingPeriodDetailsPage
{
    [Parameter]
    public int WorkDayId { get; set; }

    [Parameter]
    public int WorkingPeriodId { get; set; }

    [Inject]
    public IDialogService DialogService { get; set; }

    [Inject]
    public IMediator Mediator { get; set; }

    [Inject]
    public NavigationManager Navi { get; set; }

    public GetWorkingPeriodResponse Model { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Model = await Mediator.Send(new GetWorkingPeriodRequest(WorkDayId, WorkingPeriodId));
    }

    protected async Task HandleValidSubmitAsync()
    {
        bool result = await Mediator.Send(Model);
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
        bool result = await Mediator.Send(new DeleteWorkingPeriodRequest(Model.WorkingPeriod) { User = Model.UserContext.User });

        if (result)
        {
            Navi.NavigateTo($"workday/{Model.WorkingPeriod.WorkDayId}");
        }
    }
}