namespace WorkTimer.MediatR.Responses {
    public class AdminResponse {
        public bool HasError { get; set; }
        public string Message { get; set; }

        public static AdminResponse ErrorMessage(string message) => new AdminResponse() { HasError = true, Message = message };
    }
}
