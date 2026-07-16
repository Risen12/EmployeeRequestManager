using EmployeeRequestManager.Domain.Enums;

namespace EmployeeRequestManager.Application.DTO;

public class UpdateEmployeeRequestDto
{
    public int? NewExecutorId { get; set; }
    public string? NewStatus { get; set; }
}