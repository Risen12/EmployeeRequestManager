namespace EmployeeRequestManager.Domain.Repositories;

public class ReportDto
{
    public Dictionary<string, int> RequestsCountByStatus { get; set; } = new();
    
    public int OverdueRequestsCount { get; set; } = new();
    
    public Dictionary<string, int> CompletedRequestsByExecutor { get; set; } = new();
}