using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;
using WorkTimer.MediatR.Requests;
using WorkTimer.MediatR.Responses;

namespace WorkTimer.Blazor.Pages.Contracts
{
    public partial class EditContractPage
    {
        public GetContractResponse Model { get; set; } = default!;

        [Parameter]
        public int ContractId { get; set; }

        [Inject]
        public IMediator Mediator { get; set; } = default!;

        [Inject]
        public NavigationManager Navi { get; set; } = default!;

        [Inject]
        public IDialogService DialogService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            Model = await Mediator.Send(new GetContractRequest(ContractId));
        }

        private async Task HandleValidSubmitAsync()
        {
            bool result = await Mediator.Send(Model);
            if (result)
            {
                Navi.NavigateTo("contracts");
            }
            else
            {
                Navi.NavigateTo($"contracts/{ContractId}");
            }
        }

        void OpenDialog()
        {
            DialogParameters parameters = new()
            {
                { "DeleteEntity", OkClickAsync },
            };

            DialogService.Show<ConfirmDeletionDialog>("Delete Item", parameters);
        }

        async Task OkClickAsync()
        {
            bool result = await Mediator.Send(new DeleteContractRequest(Model) { User = Model.UserContext.User });

            if (result)
            {
                Navi.NavigateTo("/contracts");
            }
        }
    }
}