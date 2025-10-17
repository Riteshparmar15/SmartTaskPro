using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTaskPro.DTOs;
using SmartTaskPro.Services;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _svc;
    public TasksController(ITaskService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var items = await _svc.GetAllAsync(User, page, pageSize);
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var item = await _svc.GetByIdAsync(id, User);
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        var item = await _svc.CreateAsync(dto, User);
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        var item = await _svc.UpdateAsync(id, dto, User);
        return Ok(item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteAsync(id, User);
        return NoContent();
    }
}
