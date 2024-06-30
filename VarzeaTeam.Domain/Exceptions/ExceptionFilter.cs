namespace VarzeaTeam.Domain.Exceptions;

[Serializable]
public class ExceptionFilter: Exception
{
    public ExceptionFilter(string message) : base(message) { }

    public ExceptionFilter(string message, Exception inner) : base(message, inner) { }
}
