using MediatR;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WorkTimer.MediatR.Requests;

namespace WorkTimer.Blazor.Pages.Contracts
{
    public partial class NewContractPage
    {
        public NewContractRequest Model { get; set; } = new NewContractRequest();

        [Inject]
        public IMediator Mediator { get; set; } = default!;

        [Inject]
        public NavigationManager Navi { get; set; } = default!;

        private async Task HandleValidSubmitAsync()
        {
            bool result = await Mediator.Send(Model);
            if (result)
            {
                Navi.NavigateTo("contracts");
            }

            Model = new NewContractRequest();
        }
    }
}