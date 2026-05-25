using APBD_TASK7.Data;
using APBD_TASK7.DTOs;
using Microsoft.EntityFrameworkCore;
using APBD_TASK7.Models;

namespace APBD_TASK7.Services;

public class PcService : IPcService
{
    private readonly AppDbContext _context;

    public PcService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetPCResponseDTO>> GetAllPcsAsync()
    {
        return await _context.PCs
            .Select(pc => new GetPCResponseDTO
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                Components = new List<ComponentDTO>()
            })
            .ToListAsync();
    }
    public async Task<GetPcComponentsResponseDto?> GetPcComponentsAsync(int id)
    {
        return await _context.PCs
            .Where(pc => pc.Id == id)
            .Select(pc => new GetPcComponentsResponseDto
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock,
                Components = pc.PCComponents.Select(pcComponent => new PcComponentResponseDto
                {
                    Amount = pcComponent.Amount,
                    Component = new ComponentDetailsDto
                    {
                        Code = pcComponent.Component.Code,
                        Name = pcComponent.Component.Name,
                        Description = pcComponent.Component.Description,
                        Manufacturer = new ManufacturerDto
                        {
                            Id = pcComponent.Component.ComponentManufacturer.Id,
                            Abbreviation = pcComponent.Component.ComponentManufacturer.Abbreviation,
                            FullName = pcComponent.Component.ComponentManufacturer.FullName,
                            FoundationDate = pcComponent.Component.ComponentManufacturer.FoundationDate
                        },
                        Type = new ComponentTypeDto
                        {
                            Id = pcComponent.Component.ComponentType.Id,
                            Abbreviation = pcComponent.Component.ComponentType.Abbreviation,
                            Name = pcComponent.Component.ComponentType.Name
                        }
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }
    
    public async Task<GetPCResponseDTO> CreatePcAsync(CreatePcRequestDto request)
    {
        var pc = new PC
        {
            Name = request.Name,
            Weight = request.Weight,
            Warranty = request.Warranty,
            CreatedAt = DateTime.Now,
            Stock = request.Stock
        };

        _context.PCs.Add(pc);
        await _context.SaveChangesAsync();
        
        foreach (var component in request.Components)
        {
            var exists = await _context.Components
                .AnyAsync(c => c.Code == component.ComponentCode);

            if (!exists)
                throw new Exception($"Component {component.ComponentCode} not found");

            _context.PCComponents.Add(new PCComponent
            {
                PCId = pc.Id,
                ComponentCode = component.ComponentCode,
                Amount = component.Amount
            });
        }

        await _context.SaveChangesAsync();

        return new GetPCResponseDTO
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock,
            Components = request.Components.Select(c => new ComponentDTO
            {
                Code = c.ComponentCode,
                Name = "",
                Amount = c.Amount
            }).ToList()
        };
    }
    
    public async Task<bool> UpdatePcAsync(int id, UpdatePcRequestDto request)
    {
        var pc = await _context.PCs.FindAsync(id);

        if (pc == null)
            return false;

        pc.Name = request.Name;
        pc.Weight = request.Weight;
        pc.Warranty = request.Warranty;
        pc.CreatedAt = request.CreatedAt;
        pc.Stock = request.Stock;

        await _context.SaveChangesAsync();

        return true;
    }
    
    public async Task<bool> DeletePcAsync(int id)
    {
        var pc = await _context.PCs.FindAsync(id);

        if (pc == null)
            return false;

        _context.PCs.Remove(pc);
        await _context.SaveChangesAsync();

        return true;
    }
}