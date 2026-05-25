using APBD_TASK7.DTOs;

namespace APBD_TASK7.Services;

public interface IPcService
{
    Task<List<GetPCResponseDTO>> GetAllPcsAsync();
    Task<GetPcComponentsResponseDto?> GetPcComponentsAsync(int id);
    Task<GetPCResponseDTO> CreatePcAsync(CreatePcRequestDto request);
    Task<bool> UpdatePcAsync(int id, UpdatePcRequestDto request);
    Task<bool> DeletePcAsync(int id);
}