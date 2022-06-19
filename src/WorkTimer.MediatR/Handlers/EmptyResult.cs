namespace WorkTimer.MediatR.Handlers;

public class EmptyResult
{
    public static EmptyResult Empty = new();

    private EmptyResult() { }
}
