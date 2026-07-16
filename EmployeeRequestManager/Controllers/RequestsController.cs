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
        var createdRequest = await _employeeRequestService.CreateRequestAsync(request);

        return CreatedAtAction(nameof(GetEmployeeRequest), new { id = createdRequest.Id }, createdRequest);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeRequestDto>> UpdateRequest(int id, UpdateEmployeeRequestDto updateRequestDto)
    {

        if (updateRequestDto.NewExecutorId.HasValue == true)
        {
            await _employeeRequestService.ChangeRequestExecutorAsync(id, updateRequestDto.NewExecutorId.Value);
        }
        else if (updateRequestDto.NewStatus != null)
        {
            await _employeeRequestService.ChangeRequestStatusAsync(id, updateRequestDto.NewStatus);
        }
        
       var baseRequest = await _employeeRequestService.GetRequestByIdAsync(id);
        
        return Ok(baseRequest);
    }
}