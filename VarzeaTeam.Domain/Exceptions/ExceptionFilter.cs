namespace VarzeaTeam.Domain.Exceptions;

public class ExceptionFilter: Exception
{
    public ExceptionFilter() { }

    public ExceptionFilter(string message) : base(message) { }

    public ExceptionFilter(string message, Exception inner) : base(message, inner) { }
}
