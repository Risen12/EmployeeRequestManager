namespace EmployeeRequestManager.Domain.Exeptions;

public class NotFoundRequestException : Exception
{
    public NotFoundRequestException(int id) : base($"Request with id {id} was not found.")
    {
        Id = id;
    }

    public int Id { get; }
}