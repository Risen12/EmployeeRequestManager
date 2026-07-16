using EmployeeRequestManager.Domain.Entities;
using EmployeeRequestManager.Domain.Enums;

namespace EmployeeRequestManager.Application.DTO;

public class EmployeeRequestDto
{
    public int? Id { get; set; }
    public DateTime? RequestCreationDate { get; init; }
    public int AuthorId { get; init; }
    public int ExecutorId { get; init; }
    public string? AuthorName { get; init; }
    public string? ExecutorName { get; init; }
    public string Description { get; init; }
    public DateTime RequestExpirationDate { get; init; }
    public string? Status { get;  init; }
}