using MediatR;
using System.Collections.Generic;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Responses {
    public class GetWorkDayDetailsResponse : IRequest<bool> {
        public WorkDay WorkDay { get; set; }
        public ICollection<WorkingPeriod> WorkingPeriods { get; set; }
        public List<ContractDropdownListModel> Contracts { get; set; }
        public bool IsOpenWorkday { get; set; }
        public UserContext UserContext { get; set; }
    }

    public class ContractDropdownListModel {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}