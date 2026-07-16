using EmployeeRequestManager.Application.DTO;
using EmployeeRequestManager.Application.Services;
using EmployeeRequestManager.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRequestManager.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestsController : ControllerBase
{
    private readonly EmployeeRequestService  _employeeRequestService;
    
    public RequestsController(EmployeeRequestService employeeRequestService)
    {
        _employeeRequestService = employeeRequestService;
    }

    [HttpGet]
    public async Task<List<EmployeeRequestDto>> GetEmployeeRequests([FromQuery] RequestFilter? filter = null)
    {
        if (filter == null)
        {
            return await _employeeRequestService.GetAllRequestsAsync();
        }

        return await _employeeRequestService.GetRequestByFilterAsync(filter);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeRequestDto>> GetEmployeeRequest(int id)
    {
        var request = await _employeeRequestService.GetRequestByIdAsync(id);
        
        return Ok(request);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeRequestDto>> CreateRequest(EmployeeRequestDto request)
    {
        await _employeeRequestService.CreateRequestAsync(request);
        
        return  Ok(request);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeRequestDto>> UpdateRequest(int id, int newExecutorId)
    {
        var baseRequest = await _employeeRequestService.GetRequestByIdAsync(id);

        if (baseRequest.ExecutorId != newExecutorId)
        {
            await _employeeRequestService.ChangeRequestExecutorAsync(id, newExecutorId);
        }

        if (baseRequest.Status != )
        {
            await _employeeRequestService.ChangeRequestStatusAsync(id, request.Status);
        }
        
        baseRequest = await _employeeRequestService.GetRequestByIdAsync(id);
        
        return Ok(baseRequest);
    }
}