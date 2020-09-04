using MediatR;
using System.ComponentModel.DataAnnotations;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Requests {
    public class NewContractRequest : UserContext, IRequest<bool> {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Employer { get; set; }
        [Required]
        public int HoursPerWeek { get; set; }
        public bool IsCurrent { get; set; }
    }
}
