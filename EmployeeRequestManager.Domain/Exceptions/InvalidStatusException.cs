namespace EmployeeRequestManager.Domain.Exceptions;

public class InvalidStatusException : Exception
{
    public InvalidStatusException(string status) : base($"Can't recognize status {status}!") { }
}