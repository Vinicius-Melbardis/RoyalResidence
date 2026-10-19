using RoyalResidence.DTO;

namespace RoyalResidenceWeb.Services.IServices
{
    public interface IResidenceService
    {
        Task<T?> GetAllAsync<T>(string token);
        Task<T?> GetAsync<T>(int id, string token);
        Task<T?> CreateAsync<T>(ResidenceCreateDTO dto, string token);
        Task<T?> UpdateAsync<T>(ResidenceUpdateDTO dto, string token);
        Task<T?> DeleteAsync<T>(int id, string token);
    }
}
