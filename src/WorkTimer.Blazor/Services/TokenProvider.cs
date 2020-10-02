namespace WorkTimer.Blazor.Services {
    public class TokenProvider {
        public string? XsrfToken { get; set; }
    }

    public class InitialApplicationState {

        public string? XsrfToken { get; private set; }

        public InitialApplicationState(string? xsrfToken) {
            XsrfToken = xsrfToken;
        }
    }
}
