namespace apbd_task8_db_first_s28977.Exceptions;

public class ConflictRequestException : Exception
{
    public ConflictRequestException(string message) : base(message)
    {
    }
}