using System.Collections.Generic;
using WorkTimer.Domain.Models;

namespace WorkTimer.MediatR.Responses {
    public class ContractListResponse {
        public ContractListResponse(ICollection<ContractListModel> contracts) {
            Contracts = contracts;
        }

        public ICollection<ContractListModel> Contracts { get; set; }

        public class ContractListModel : Contract {
            public ContractListModel(Contract contract) {
                Id = contract.Id;
                Name = contract.Name;
                Employer = contract.Employer;
                HoursPerWeek = contract.HoursPerWeek;
                IsCurrent = contract.IsCurrent;
                UserId = contract.UserId;
            }
        }
    }
}
