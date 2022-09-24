namespace WorkTimer.Blazor.Services;
public class TokenProvider
{
    public string? XsrfToken { get; set; }
}

public class InitialApplicationState
{
    public InitialApplicationState(string? xsrfToken)
    {
        XsrfToken = xsrfToken;
    }

    public string? XsrfToken { get; }
}