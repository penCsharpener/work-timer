using MediatR;
using System.ComponentModel.DataAnnotations;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Responses
{
    public class GetContractResponse : UserContext, IRequest<bool>
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Employer { get; set; }

        [Required]
        public int HoursPerWeek { get; set; }

        public bool IsCurrent { get; set; }

        public UserContext UserContext { get; set; }
    }
}