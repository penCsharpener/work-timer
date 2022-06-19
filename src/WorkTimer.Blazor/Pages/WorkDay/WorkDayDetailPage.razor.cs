using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Requests;
using WorkTimer.MediatR.Responses;

namespace WorkTimer.Blazor.Pages.WorkDay
{
    public partial class WorkDayDetailPage
    {
        [Parameter]
        public int WorkDayId { get; set; }

        [Inject]
        public IDialogService DialogService { get; set; } = default!;

        public GetWorkDayDetailsResponse Model { get; set; }

        public WorkDayType[] WorkDayTypes { get; set; } = Enum.GetValues(typeof(WorkDayType)).Cast<WorkDayType>().ToArray();

        private MudTable<WorkingPeriod> _table;

        protected override async Task OnInitializedAsync()
        {
            Model = await Mediator.Send(new GetWorkDayDetailsRequest(WorkDayId));
        }

        protected async Task HandleValidSubmitAsync()
        {
            bool result = await Mediator.Send(Model);
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
            bool result = await Mediator.Send(new DeleteWorkDayRequest(Model.WorkDay) { User = Model.UserContext.User });

            if (result)
            {
                Navi.NavigateTo("/");
            }
        }
    }
}