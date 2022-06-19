using System.Threading.Tasks;
using WorkTimer.MediatR.Handlers;

namespace WorkTimer.Blazor.Pages.Admin;

public partial class AdminPage
{
    public AdminRequest RequestModel { get; set; } = new AdminRequest();
    public AdminResponse ResponseModel { get; set; } = new AdminResponse();
    private string _errorTextStyle = "color: var(--mud-palette-info-text);";
    private async Task HandleValidSubmitAsync()
    {
        ResponseModel = await Mediator.Send(RequestModel);
        _errorTextStyle = ResponseModel.HasError ? "color: var(--mud-palette-error);" : "color: var(--mud-palette-info-text);";
    }
}