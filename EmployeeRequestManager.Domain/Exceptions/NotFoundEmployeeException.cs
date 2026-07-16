namespace EmployeeRequestManager.Domain.Exeptions;

public class NotFoundEmployeeException : Exception
{
    public NotFoundEmployeeException(int id) : base($"Employee with id {id} was not found.")
    {
        Id = id;
    }

    public int Id { get; }
}