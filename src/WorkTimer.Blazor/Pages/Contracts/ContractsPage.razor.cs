using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;
using WorkTimer.MediatR.Handlers;

namespace WorkTimer.Blazor.Pages.Contracts;

public partial class ContractsPage
{
    public ContractListResponse? Response { get; set; }

    [Inject]
    public IMediator Mediator { get; set; } = default!;

    [Inject]
    public NavigationManager Navi { get; set; } = default!;

    private readonly MudTheme Theme = new();
    protected override async Task OnInitializedAsync()
    {
        Response = await Mediator.Send(new ContractListRequest());
    }

    private string GetCardColor(bool isCurrentContract)
    {
        return isCurrentContract ? $"background-color: {Theme.Palette.BackgroundGrey};" : $"background-color: {Theme.Palette.Background};";
    }

    private string RedirectToCreateContract()
    {
        Navi.NavigateTo("/contracts/add");
        return string.Empty;
    }
}