﻿namespace WorkTimer.MediatR.Responses {
    public class AdminResponse {
        public bool HasError { get; set; }
        public string Message { get; set; }

        public static AdminResponse ErrorMessage(string message) {
            return new AdminResponse { HasError = true, Message = message };
        }
    }
}