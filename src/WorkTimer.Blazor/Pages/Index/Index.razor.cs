using MudBlazor;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.MediatR.Handlers;

namespace WorkTimer.Blazor.Pages.Index;

public partial class Index
{
    public IndexResponse IndexResponse { get; set; }

    public NewWorkingPeriodRequest NewPeriodModel { get; set; } = new NewWorkingPeriodRequest();
    public string dayButtonText;
    private MudTable<DisplayWorkDayModel> _tableWorkdays;
    private MudTable<Domain.Models.WorkingPeriod> _tableRecent;
    readonly int pageSize = 25;
    readonly int pageIndex;
    protected override async Task OnInitializedAsync()
    {
        IndexResponse = await Mediator.Send(new IndexRequest(pageIndex, pageSize));
        EvaluateButtonText();
        await base.OnInitializedAsync();
    }

    private void PageChanged(int i)
    {
        _tableWorkdays.NavigateTo(i - 1);
    }

    private async Task HandleValidSubmitAsync()
    {
        await Mediator.Send(NewPeriodModel);
        NewPeriodModel = new NewWorkingPeriodRequest();
        IndexResponse = await Mediator.Send(new IndexRequest());
        EvaluateButtonText();
        Navi.NavigateTo("/");
    }

    private string CheckForContracts()
    {
        var contract = Mediator.Send(new ContractListRequest()).GetAwaiter().GetResult();
        if (contract?.Contracts?.Any() == false)
        {
            Navi.NavigateTo("/contracts/add");
        }

        return "";
    }

    private void EvaluateButtonText()
    {
        dayButtonText = IndexResponse.HasOngoingWorkPeriod ? "Stop" : "Start";
    }
}