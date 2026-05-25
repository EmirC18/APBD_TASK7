using APBD_TASK7.Services;
using Microsoft.AspNetCore.Mvc;
using APBD_TASK7.DTOs;

namespace APBD_TASK7.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PCsController : ControllerBase
{
    private readonly IPcService _pcService;

    public PCsController(IPcService pcService)
    {
        _pcService = pcService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPcs()
    {
        var pcs = await _pcService.GetAllPcsAsync();

        return Ok(pcs);
    }
    
    [HttpGet("{id}/components")]
    public async Task<IActionResult> GetPcComponents(int id)
    {
        var pc = await _pcService.GetPcComponentsAsync(id);

        if (pc == null)
            return NotFound();

        return Ok(pc);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePc(CreatePcRequestDto request)
    {
        var result = await _pcService.CreatePcAsync(request);

        return Created($"api/pcs/{result.Id}", result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePc(int id, UpdatePcRequestDto request)
    {
        var updated = await _pcService.UpdatePcAsync(id, request);

        if (!updated)
            return NotFound();

        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePc(int id)
    {
        var deleted = await _pcService.DeletePcAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}