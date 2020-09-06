namespace WorkTimer.MediatR.Models {
    public class RestrictRegistration {
        public bool TotalLockDown { get; set; }
        public string[] PermittedEmails { get; set; }
        public string[] PermittedDomains { get; set; }
        public string[] BlockedDomains { get; set; }
        public string[] BlockedEmails { get; set; }
    }
}
