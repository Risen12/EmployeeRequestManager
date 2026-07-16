using EmployeeRequestManager.Domain.Entities;
using EmployeeRequestManager.Domain.Enums;

namespace EmployeeRequestManager.Application.DTO;

public class EmployeeRequestDto
{
    public int Id { get; set; }
    public DateTime RequestCreationDate { get; init; }
    public int AuthorId { get; init; }
    public int ExecutorId { get; init; }
    public string Description { get; init; }
    public DateTime RequestExpirationDate { get; init; }
    public RequestStatus Status { get;  init; }
}