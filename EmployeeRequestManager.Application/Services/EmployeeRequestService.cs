using EmployeeRequestManager.Application.DTO;
using EmployeeRequestManager.Domain.Entities;
using EmployeeRequestManager.Domain.Enums;
using EmployeeRequestManager.Domain.Exeptions;
using EmployeeRequestManager.Domain.Repositories;

namespace EmployeeRequestManager.Application.Services;

public class EmployeeRequestService
{
    private readonly IEmployeeRequestRepository _employeeRequestRepository;
    private readonly IEmployeeRepository _employeeRepository;
    
    public EmployeeRequestService(IEmployeeRequestRepository repository,  IEmployeeRepository employeeRepository)
    {
        _employeeRequestRepository = repository;
        _employeeRepository = employeeRepository;
    }

    public async Task<EmployeeRequestDto> GetRequestByIdAsync(int id)
    {
        var request = await _employeeRequestRepository.GetRequestByIdAsync(id);
        
        return ConvertToDto(request);
    }

    public async Task<List<EmployeeRequestDto>> GetAllRequestsAsync()
    {
        var requests = await _employeeRequestRepository.GetAllRequestsAsync();
        
        return await ConvertToDto(requests);
    }

    public async Task<EmployeeRequestDto> CreateRequestAsync(EmployeeRequestDto employeeRequest)
    {
        var request = new EmployeeRequest()
        {
            RequestCreationDate = employeeRequest.RequestCreationDate,
            RequestExpirationDate = employeeRequest.RequestExpirationDate,
            Description = employeeRequest.Description,
            Status = employeeRequest.Status
        };
        
        var author = await _employeeRepository.GetEmployeeByIdAsync(employeeRequest.AuthorId);
        var executor = await _employeeRepository.GetEmployeeByIdAsync(employeeRequest.ExecutorId);

        request.Executor = executor;
        request.Author = author;

        await _employeeRequestRepository.CreateRequestAsync(request);
        
        return ConvertToDto(request);
    }

    public async Task<List<EmployeeRequestDto>> GetRequestByFilterAsync(RequestFilter filter)
    {
        var requests =  await _employeeRequestRepository.GetRequestByFilterAsync(filter);
        
        return await ConvertToDto(requests);
    }

    public async Task<EmployeeRequestDto> ChangeRequestStatusAsync(int id, RequestStatus requestStatus)
    {
        var request = await _employeeRequestRepository.GetRequestByIdAsync(id);
        
        request.ChangeStatus(requestStatus);
        
        await _employeeRequestRepository.UpdateRequestAsync(request);
        
        return ConvertToDto(request);
    }

    public async Task<EmployeeRequestDto> ChangeRequestExecutorAsync(int id, int newExecutorId)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(newExecutorId);

        var request = await _employeeRequestRepository.GetRequestByIdAsync(id);
        
        request.ChangeExecutor(employee);
        await _employeeRequestRepository.UpdateRequestAsync(request);
        
        return ConvertToDto(request);
    }

    private EmployeeRequestDto ConvertToDto(EmployeeRequest request)
    {
        return new EmployeeRequestDto
        {
            Id = request.Id,
            RequestCreationDate = request.RequestCreationDate,
            RequestExpirationDate = request.RequestExpirationDate,
            Description = request.Description,
            Status = request.Status,
            AuthorId = request.Author.Id,
            ExecutorId = request.Executor.Id
        };
    }

    private async Task<List<EmployeeRequestDto>> ConvertToDto(IEnumerable<EmployeeRequest> requests)
    {
        var resultRequests = new  List<EmployeeRequestDto>();
        
        foreach (var request in requests)
        {
            resultRequests.Add(ConvertToDto(request));
        }
        
        return resultRequests;
    }
}