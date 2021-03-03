using WorkTimer.Domain.Models;

namespace WorkTimer.MediatR.Models
{
    public class UserContext
    {
        public string UserEmail { get; set; }
        public bool UserIsAdmin { get; set; }
        public AppUser User { get; set; }
        public Contract CurrentContract { get; set; }
    }
}