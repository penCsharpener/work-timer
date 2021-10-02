namespace WorkTimer.MediatR.Responses
{
    public class EmptyResult
    {
        public static EmptyResult Empty = new EmptyResult();

        private EmptyResult() { }
    }
}
