using MediatR;

namespace WorkTimer.MediatR.Requests {
    public class RestrictRegistrationRequest : IRequest<bool> {
        public RestrictRegistrationRequest(string userEmail) {
            UserEmail = userEmail;
        }

        public string UserEmail { get; set; }
    }
}